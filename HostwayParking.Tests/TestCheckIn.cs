using HostwayParking.Business.Exceptions;
using HostwayParking.Business.UseCase.Session.Check_In;
using HostwayParking.Communication.Request;
using HostwayParking.Domain.Entities;
using HostwayParking.Domain.Interface;
using Moq;

namespace HostwayParking.Tests
{
    public class TestCheckIn
    {
        private readonly Mock<ISessionParkingRepository> _sessionRepoMock;
        private readonly Mock<IVehiclesRepository> _vehicleRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CheckInSessionUseCase _useCase;

        public TestCheckIn()
        {
            _sessionRepoMock = new Mock<ISessionParkingRepository>();
            _vehicleRepoMock = new Mock<IVehiclesRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _useCase = new CheckInSessionUseCase(
                _sessionRepoMock.Object,
                _vehicleRepoMock.Object,
                _unitOfWorkMock.Object);
        }

        private static RequestRegisterCheckInJson ValidRequest() => new()
        {
            Plate = "ABC1D23",
            Model = "Civic",
            Color = "Preto",
            Type = "Carro"
        };

        // --- Cenários de sucesso ---

        [Fact]
        public async Task Execute_VeiculoNovo_SemSessaoAtiva_CriaVeiculoESessao()
        {
            var request = ValidRequest();

            _sessionRepoMock
                .Setup(r => r.GetActiveSessionByPlateAsync(request.Plate))
                .ReturnsAsync((SessionParking)null!);

            _vehicleRepoMock
                .Setup(r => r.GetByPlateAsync(request.Plate))
                .ReturnsAsync((Vehicle)null!);

            await _useCase.Execute(request);

            _vehicleRepoMock.Verify(r => r.Post(It.Is<Vehicle>(v =>
                v.Plate == request.Plate &&
                v.Model == request.Model &&
                v.Color == request.Color &&
                v.Type == request.Type)), Times.Once);

            _sessionRepoMock.Verify(r => r.AddAsync(It.IsAny<SessionParking>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Exactly(2));
        }

        [Fact]
        public async Task Execute_VeiculoExistente_SemSessaoAtiva_CriaSomenteASessao()
        {
            var request = ValidRequest();
            var existingVehicle = new Vehicle { Id = 10, Plate = request.Plate, Model = request.Model, Color = request.Color, Type = request.Type };

            _sessionRepoMock
                .Setup(r => r.GetActiveSessionByPlateAsync(request.Plate))
                .ReturnsAsync((SessionParking)null!);

            _vehicleRepoMock
                .Setup(r => r.GetByPlateAsync(request.Plate))
                .ReturnsAsync(existingVehicle);

            await _useCase.Execute(request);

            _vehicleRepoMock.Verify(r => r.Post(It.IsAny<Vehicle>()), Times.Never);
            _sessionRepoMock.Verify(r => r.AddAsync(It.Is<SessionParking>(s => s.VehicleId == existingVehicle.Id)), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        // --- Veículo já está no pátio ---

        [Fact]
        public async Task Execute_SessaoAtivaExistente_LancaExcecao()
        {
            var request = ValidRequest();
            var activeSession = new SessionParking(5);

            _sessionRepoMock
                .Setup(r => r.GetActiveSessionByPlateAsync(request.Plate))
                .ReturnsAsync(activeSession);

            var ex = await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(request));
            Assert.Equal("Veículo já está no pátio!", ex.Message);

            _vehicleRepoMock.Verify(r => r.GetByPlateAsync(It.IsAny<string>()), Times.Never);
            _sessionRepoMock.Verify(r => r.AddAsync(It.IsAny<SessionParking>()), Times.Never);
        }

        // --- Validação de placa ---

        [Theory]
        [InlineData("")]
        [InlineData("INVALID")]
        [InlineData("12345")]
        public async Task Execute_PlacaInvalida_LancaValidationErrorsException(string plate)
        {
            var request = ValidRequest();
            request.Plate = plate;

            var ex = await Assert.ThrowsAsync<ValidationErrorsException>(() => _useCase.Execute(request));
            Assert.NotEmpty(ex.Errors);
        }

        // --- Validação de modelo ---

        [Fact]
        public async Task Execute_ModeloVazio_LancaValidationErrorsException()
        {
            var request = ValidRequest();
            request.Model = string.Empty;

            var ex = await Assert.ThrowsAsync<ValidationErrorsException>(() => _useCase.Execute(request));
            Assert.Contains(ex.Errors, e => e.Contains("modelo"));
        }

        [Fact]
        public async Task Execute_ModeloExcedeTamanhoMaximo_LancaValidationErrorsException()
        {
            var request = ValidRequest();
            request.Model = new string('A', 51);

            var ex = await Assert.ThrowsAsync<ValidationErrorsException>(() => _useCase.Execute(request));
            Assert.Contains(ex.Errors, e => e.Contains("modelo"));
        }

        // --- Validação de cor ---

        [Fact]
        public async Task Execute_CorVazia_LancaValidationErrorsException()
        {
            var request = ValidRequest();
            request.Color = string.Empty;

            var ex = await Assert.ThrowsAsync<ValidationErrorsException>(() => _useCase.Execute(request));
            Assert.Contains(ex.Errors, e => e.Contains("cor"));
        }

        [Fact]
        public async Task Execute_CorExcedeTamanhoMaximo_LancaValidationErrorsException()
        {
            var request = ValidRequest();
            request.Color = new string('A', 31);

            var ex = await Assert.ThrowsAsync<ValidationErrorsException>(() => _useCase.Execute(request));
            Assert.Contains(ex.Errors, e => e.Contains("cor"));
        }

        // --- Validação de tipo ---

        [Fact]
        public async Task Execute_TipoVazio_LancaValidationErrorsException()
        {
            var request = ValidRequest();
            request.Type = string.Empty;

            var ex = await Assert.ThrowsAsync<ValidationErrorsException>(() => _useCase.Execute(request));
            Assert.Contains(ex.Errors, e => e.Contains("tipo"));
        }

        [Fact]
        public async Task Execute_TipoExcedeTamanhoMaximo_LancaValidationErrorsException()
        {
            var request = ValidRequest();
            request.Type = new string('A', 21);

            var ex = await Assert.ThrowsAsync<ValidationErrorsException>(() => _useCase.Execute(request));
            Assert.Contains(ex.Errors, e => e.Contains("tipo"));
        }

        // --- Múltiplos campos inválidos ---

        [Fact]
        public async Task Execute_TodosCamposVazios_LancaValidationErrorsExceptionComMultiplosErros()
        {
            var request = new RequestRegisterCheckInJson
            {
                Plate = "",
                Model = "",
                Color = "",
                Type = ""
            };

            var ex = await Assert.ThrowsAsync<ValidationErrorsException>(() => _useCase.Execute(request));
            Assert.True(ex.Errors.Count > 1);
        }

        // --- Nenhuma interação com repositório quando validação falha ---

        [Fact]
        public async Task Execute_ValidacaoFalha_NaoInterageComRepositorios()
        {
            var request = new RequestRegisterCheckInJson
            {
                Plate = "",
                Model = "",
                Color = "",
                Type = ""
            };

            await Assert.ThrowsAsync<ValidationErrorsException>(() => _useCase.Execute(request));

            _sessionRepoMock.Verify(r => r.GetActiveSessionByPlateAsync(It.IsAny<string>()), Times.Never);
            _vehicleRepoMock.Verify(r => r.GetByPlateAsync(It.IsAny<string>()), Times.Never);
            _vehicleRepoMock.Verify(r => r.Post(It.IsAny<Vehicle>()), Times.Never);
            _sessionRepoMock.Verify(r => r.AddAsync(It.IsAny<SessionParking>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
        }

        // --- Formatos de placa válidos ---

        [Theory]
        [InlineData("ABC1D23")]
        [InlineData("ABC1234")]
        [InlineData("AB123CD")]
        [InlineData("ABC123")]
        public async Task Execute_FormatosValidosDePlaca_NaoLancaErroDeValidacao(string plate)
        {
            var request = ValidRequest();
            request.Plate = plate;

            _sessionRepoMock
                .Setup(r => r.GetActiveSessionByPlateAsync(plate))
                .ReturnsAsync((SessionParking)null!);

            _vehicleRepoMock
                .Setup(r => r.GetByPlateAsync(plate))
                .ReturnsAsync(new Vehicle { Id = 1, Plate = plate });

            await _useCase.Execute(request);

            _sessionRepoMock.Verify(r => r.AddAsync(It.IsAny<SessionParking>()), Times.Once);
        }
    }
}
