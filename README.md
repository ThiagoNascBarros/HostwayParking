# 🚗 Hostway Parking

Sistema de gestão de estacionamento desenvolvido como parte da **3ª Etapa do Processo de Seleção 2026 da Envvio**. O projeto consiste numa solução completa (Fullstack) para o controlo de entrada e saída de veículos, com cálculo automático de tarifas e relatórios detalhados, preparado para operação nos mercados do Brasil e Argentina.

## 🛠️ Tecnologias e Ferramentas

### **Backend**
* **ASP.NET Core Web API (.NET 10):** Estrutura robusta para a API.
* **Entity Framework Core:** Utilizado para a abstração e manipulação do banco de dados.
* **SQLite:** Persistência de dados local conforme requisito do desafio.
* **FluentValidation:** Implementação de regras de validação de entrada de dados.
* **Scalar:** Interface interativa para documentação e teste dos endpoints.

### **Frontend**
* **Angular 21:** Framework para a construção de uma interface moderna e reativa.
* **TypeScript & RxJS:** Tipagem forte e gestão eficiente de fluxos assíncronos.
* **Sass (SCSS):** Estilização avançada e modular.

---

## 🚀 Funcionalidades

### 1. Gestão de Veículos
* Cadastro de veículos com campos obrigatórios: Placa, Modelo, Cor e Tipo.
* Garantia de **unicidade de placa** no banco de dados.
* Operações de listagem e edição de dados.

### 2. Controlo de Movimentação (Sessões)
* **Check-in:** Registo de entrada de veículos. O sistema impede a entrada se o veículo já possuir uma sessão ativa.
* **Check-out:** Finalização da estadia com registo automático de data/hora e valor.
* **Prévia de Valor:** Consulta do valor acumulado antes da confirmação da saída.

### 3. Relatórios Analíticos
* **Faturamento:** Consulta de receita total agrupada por dia (últimos 7 ou 30 dias).
* **Top 10 Veículos:** Ranking dos veículos com maior tempo de permanência num período selecionado.
* **Taxa de Ocupação:** Análise de quantos veículos estiveram no pátio por hora do dia.

---

## 💰 Regras de Negócio e Precificação

O cálculo de valores segue a lógica estabelecida no desafio:
* **Primeira hora:** R$ 10,00.
* **Horas adicionais:** R$ 5,00 por hora.
* **Arredondamento:** O sistema utiliza o método de **arredondamento para cima (teto)**. Qualquer fração de hora adicional é cobrada como uma hora integral para garantir a rentabilidade da operação.

---

## 📦 Estrutura do Projeto

O backend utiliza uma arquitetura em camadas para separação de responsabilidades:
* `HostwayParking.Api`: Controladores e configuração da aplicação.
* `HostwayParking.Business`: Lógica de negócio, Casos de Uso e Validadores.
* `HostwayParking.Domain`: Entidades, Interfaces e regras de domínio.
* `HostwayParking.Infrastructure`: Persistência (SQLite), Contexto de Dados e Repositórios.
* `HostwayParking.Communication`: DTOs para comunicação entre Front e Back.

---

## ⚡ Como Executar

### Pré-requisitos
* .NET 10 SDK
* Node.js (v18+) e npm
* Angular CLI

### 1. Backend
```bash
cd HostwayParking
dotnet run
```

- A API estará disponível em: https://localhost:7062

- Documentação Scalar: https://localhost:7062/scalar/v1

### 2. Frontend
```bash
cd HostwayParking.UI
npm install
npm start
```

- Aceda ao sistema em: http://localhost:4200

### 🧪 Testes Automatizados
Foram implementados testes unitários para validar as regras críticas de domínio e casos de uso (ex: lógica de check-in e validações de placa):

```Bash
cd HostwayParking.Tests
dotnet test
```
