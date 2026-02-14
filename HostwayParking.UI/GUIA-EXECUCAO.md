# 🚗 Hostway Parking - Guia de Execução Completo

## ⚡ Início Rápido

### Passo 1: Iniciar o Backend (.NET)

```bash
# Navegue até o diretório do backend
cd c:\Users\tdona\source\repos\HostwayParking\HostwayParking

# Execute o backend
dotnet run
```

**O backend estará disponível em:** `https://localhost:7185`

### Passo 2: Iniciar o Frontend (Angular)

**Abra um novo terminal** e execute:

```bash
# Navegue até o diretório do frontend
cd c:\Users\tdona\source\repos\HostwayParking\HostwayParking.UI

# Instale as dependências (primeira vez apenas)
npm install

# Execute o frontend
npm start
```

**O frontend estará disponível em:** `http://localhost:4200`

## 🌐 Acessar o Sistema

Abra seu navegador e acesse: **http://localhost:4200**

## ✅ Checklist de Verificação

- [ ] Backend rodando em https://localhost:7185
- [ ] Frontend rodando em http://localhost:4200
- [ ] Navegador aberto em http://localhost:4200
- [ ] Sem erros de CORS no console do navegador

## 🔄 Fluxo de Uso do Sistema

### 1. Cadastrar Estacionamento
1. Acesse "Estacionamentos" no menu
2. Clique em "Cadastrar Estacionamento"
3. Preencha os dados e salve

### 2. Cadastrar Veículo (Opcional)
1. Acesse "Veículos" no menu
2. Clique em "+ Adicionar Veículo"
3. Preencha placa, modelo, cor e tipo

### 3. Fazer Check-in
1. Acesse "Sessões" no menu
2. Preencha os dados do veículo no formulário "Check-In"
3. Clique em "Fazer Check-In"

### 4. Fazer Check-out
1. Na lista de "Sessões Ativas", encontre o veículo
2. Clique em "Prévia" para ver o valor a pagar
3. Clique em "Confirmar Check-Out" para finalizar

### 5. Visualizar Relatórios
1. Acesse "Relatórios" no menu
2. Escolha entre:
   - Receita por Dia
   - Veículos que Mais Permaneceram
   - Taxa de Ocupação por Hora

## 🐛 Solução de Problemas

### Backend não inicia
```bash
# Verifique se o .NET está instalado
dotnet --version

# Restaure os pacotes
dotnet restore
```

### Frontend apresenta erro de CORS
- Certifique-se de que o backend está rodando
- Verifique se a URL do backend é `https://localhost:7185`
- Verifique a configuração CORS em `Program.cs`

### Erro "Cannot find module"
```bash
# Reinstale as dependências
cd HostwayParking.UI
rm -rf node_modules
npm install
```

## 📊 Estrutura dos Dados

### Exemplo de Payload - Check-in
```json
{
  "plate": "ABC-1234",
  "model": "Honda Civic",
  "color": "Preto",
  "type": "Car"
}
```

### Tipos de Veículo
- `Motorcycle` - Moto
- `Car` - Carro

## 🎯 Recursos Principais

✅ Gerenciamento completo de estacionamentos  
✅ Cadastro e edição de veículos  
✅ Sistema de check-in/check-out com preview  
✅ Cálculo automático de valores  
✅ Relatórios de receita e ocupação  
✅ Interface responsiva e moderna  
✅ Validação de formulários  
✅ Feedback visual de operações  

## 📱 Navegação do Sistema

```
Dashboard (/)
├── Sessões (/sessions)
│   ├── Check-in
│   ├── Sessões Ativas
│   └── Check-out
├── Veículos (/vehicles)
│   ├── Lista (/vehicles)
│   └── Cadastrar (/vehicles/new)
├── Estacionamentos (/parkings)
│   ├── Lista (/parkings)
│   └── Cadastrar (/parkings/new)
└── Relatórios (/reports)
    ├── Receita por Dia
    ├── Top Veículos
    └── Taxa de Ocupação
```

---

**Desenvolvido para o Processo Seletivo Hostway 2026**
