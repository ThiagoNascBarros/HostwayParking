# Hostway Parking - Frontend

Sistema de gerenciamento de estacionamento desenvolvido em Angular 21.

## 🚀 Como Executar o Projeto

### Pré-requisitos

- Node.js (versão 18 ou superior)
- npm (versão 10 ou superior)
- Angular CLI (`npm install -g @angular/cli`)

### Instalação

1. Instale as dependências:
```bash
npm install
```

2. Execute o projeto em modo de desenvolvimento:
```bash
npm start
```

O aplicativo estará disponível em `http://localhost:4200/`

## 🔌 Conexão com o Backend

### Configuração da API

O frontend está configurado para se conectar ao backend através do serviço `ApiService` localizado em:
- Arquivo: `src/app/services/api.service.ts`
- URL Base: `https://localhost:7185/api`

### Como Iniciar o Backend

1. Navegue até o diretório do backend:
```bash
cd ..\HostwayParking
```

2. Execute o backend:
```bash
dotnet run
```

O backend estará disponível em `https://localhost:7185`

### CORS

O backend já está configurado para aceitar requisições do frontend Angular:
- Origem permitida: `http://localhost:4200`
- Configuração em: `HostwayParking\Program.cs`

## 📁 Estrutura do Projeto

```
src/
├── app/
│   ├── components/          # Componentes da aplicação
│   │   ├── dashboard/       # Página inicial
│   │   ├── parking-list/    # Lista de estacionamentos
│   │   ├── parking-register/# Cadastro de estacionamento
│   │   ├── vehicle-list/    # Lista de veículos
│   │   ├── vehicle-create/  # Cadastro de veículo
│   │   ├── session-management/ # Check-in/Check-out
│   │   └── reports/         # Relatórios
│   ├── models/              # Interfaces TypeScript
│   │   ├── parking.model.ts
│   │   ├── vehicle.model.ts
│   │   ├── session.model.ts
│   │   └── report.model.ts
│   ├── services/            # Serviços Angular
│   │   └── api.service.ts   # Comunicação com API
│   ├── app.config.ts        # Configuração da aplicação
│   ├── app.routes.ts        # Rotas
│   ├── app.ts               # Componente raiz
│   ├── app.html             # Template raiz
│   └── app.scss             # Estilos raiz
├── index.html
├── main.ts
└── styles.scss              # Estilos globais
```

## 🎯 Funcionalidades

### 1. Dashboard
- Página inicial com acesso rápido a todas as funcionalidades

### 2. Gestão de Estacionamentos
- **Listar**: Visualizar todos os estacionamentos cadastrados
- **Cadastrar**: Adicionar novo estacionamento com:
  - Nome
  - Endereço
  - Número de vagas (motos e carros)
  - Preço por hora (motos e carros)

### 3. Gestão de Veículos
- **Listar**: Visualizar todos os veículos cadastrados
- **Cadastrar**: Adicionar novo veículo com placa, modelo, cor e tipo
- **Editar**: Atualizar informações do veículo

### 4. Gestão de Sessões
- **Check-in**: Registrar entrada de veículo
- **Visualizar Sessões Ativas**: Ver todos os veículos estacionados
- **Prévia de Check-out**: Visualizar valor a pagar antes de finalizar
- **Check-out**: Finalizar estacionamento e processar pagamento

### 5. Relatórios
- **Receita por Dia**: Visualizar receita dos últimos X dias
- **Veículos que Mais Permaneceram**: Top veículos por tempo de permanência
- **Taxa de Ocupação por Hora**: Análise de ocupação do estacionamento

## 🔧 Endpoints da API

### Parking
- `POST /api/Parking` - Cadastrar estacionamento
- `GET /api/Parking` - Listar estacionamentos

### Vehicle
- `POST /api/Vehicle` - Cadastrar veículo
- `PUT /api/Vehicle/{plate}` - Atualizar veículo
- `GET /api/Vehicle` - Listar veículos

### SessionParking
- `POST /api/SessionParking/checkin` - Fazer check-in
- `POST /api/SessionParking/checkout` - Fazer check-out
- `GET /api/SessionParking` - Listar sessões ativas
- `GET /api/SessionParking/checkout/preview/{plate}` - Prévia do check-out

### Report
- `GET /api/Report/revenue?days={days}` - Receita por dia
- `GET /api/Report/top-vehicles?start={start}&end={end}` - Top veículos
- `GET /api/Report/occupancy?start={start}&end={end}` - Taxa de ocupação

## 🛠️ Desenvolvimento

### Gerar novo componente
```bash
ng generate component components/nome-componente
```

### Gerar novo serviço
```bash
ng generate service services/nome-servico
```

### Build para produção
```bash
ng build
```

Os arquivos de build serão armazenados em `dist/`

## 🧪 Testes

```bash
npm test
```

## 📝 Notas Importantes

1. **Certifique-se de que o backend está rodando** antes de iniciar o frontend
2. A porta do backend deve ser `7185` (HTTPS)
3. A porta do frontend deve ser `4200`
4. Em caso de erro de CORS, verifique as configurações em `Program.cs` do backend

## 🎨 Tecnologias Utilizadas

- Angular 21.1.0
- TypeScript 5.9.2
- RxJS 7.8.0
- Angular Router
- Angular Forms
- HttpClient com Fetch API
