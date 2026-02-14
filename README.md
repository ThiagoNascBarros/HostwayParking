# HostwayParking API — Documentação Completa para Front-End Angular

## Visão Geral

API REST para gerenciamento de estacionamento (parking lot). Backend em **.NET 10** com **Entity Framework Core** e banco **SQLite**. A API gerencia pátios (parkings), veículos, sessões de estacionamento (check-in / check-out) e relatórios analíticos.

**Base URL:** `http://localhost:5217` (ou `https://localhost:7062`)
**CORS:** Configurado para `http://localhost:4200` (Angular dev server)

---

## Entidades do Domínio (para referência)

### Parking (Pátio)
| Campo | Tipo | Descrição |
|-------|------|-----------|
| id | int | PK auto-increment |
| code | string (max 12) | Código único do pátio |
| address.state | string | Estado |
| address.city | string | Cidade |
| address.supplement | string | Complemento |
| address.number | string | Número |
| vehicles | Vehicle[] | Veículos associados ao pátio |

### Vehicle (Veículo)
| Campo | Tipo | Descrição |
|-------|------|-----------|
| id | int | PK auto-increment |
| plate | string (max 7, unique) | Placa do veículo (ex: "ABC1D23") |
| model | string | Modelo (ex: "Civic") |
| color | string | Cor (ex: "Preto") |
| type | string | Tipo (ex: "Carro", "Moto") |
| parkingId | int? | FK para o pátio (nullable) |

### SessionParking (Sessão de Estacionamento)
| Campo | Tipo | Descrição |
|-------|------|-----------|
| id | int | PK auto-increment |
| vehicleId | int | FK para veículo |
| entryTime | DateTime | Data/hora de entrada |
| exitTime | DateTime? | Data/hora de saída (null = ainda no pátio) |
| amountCharged | decimal? | Valor cobrado no checkout (null = sessão ativa) |
| isActive | bool (computed) | `true` se `exitTime == null` |

### Regra de Preço
- **Primeira hora:** R$ 10,00
- **Hora adicional:** R$ 5,00 por hora (arredondado para cima)

---

## Endpoints

### 1. Parking (Pátio)

#### `POST /api/Parking` — Registrar pátio
**Request Body:**
```json
{
  "code": "PARK-001",
  "address": {
    "state": "SP",
    "city": "São Paulo",
    "supplement": "Bloco A",
    "number": "123"
  }
}
```
**Response 200:**
```json
{
  "code": "PARK-001"
}
```
**Response 400:** `"mensagem de erro"`

---

#### `GET /api/Parking` — Listar todos os pátios
**Response 200:**
```json
[
  {
    "code": "PARK-001",
    "city": "São Paulo"
  }
]
```

---

### 2. Vehicle (Veículo)

#### `POST /api/Vehicle` — Cadastrar veículo
**Request Body:**
```json
{
  "plate": "ABC1D23",
  "model": "Civic",
  "color": "Preto",
  "type": "Carro"
}
```
**Response 201:** sem body (Created)
**Response 400:** `"mensagem de erro"`

---

#### `PUT /api/Vehicle/{plate}` — Atualizar veículo
**URL Param:** `plate` (string) — placa do veículo a atualizar
**Request Body:**
```json
{
  "plate": "ABC1D23",
  "model": "Civic Sport",
  "color": "Branco",
  "type": "Carro"
}
```
**Response 204:** sem body (NoContent)
**Response 400:** `"mensagem de erro"`

---

#### `GET /api/Vehicle` — Listar todos os veículos
**Response 200:**
```json
[
  {
    "id": 1,
    "plate": "ABC1D23",
    "model": "Civic",
    "color": "Preto",
    "type": "Carro"
  }
]
```

---

### 3. SessionParking (Sessões — Check-in / Check-out)

#### `POST /api/SessionParking/checkin` — Registrar entrada
**Request Body:**
```json
{
  "plate": "ABC1D23",
  "model": "Civic",
  "color": "Preto",
  "type": "Carro"
}
```
**Response 201:** sem body (Created)
**Response 400:** `"mensagem de erro"`

> **Nota:** Se o veículo não existir no banco, ele é criado automaticamente no check-in.

---

#### `POST /api/SessionParking/checkout` — Registrar saída (finaliza sessão)
**Request Body:**
```json
{
  "plate": "ABC1D23"
}
```
**Response 200:**
```json
{
  "plate": "ABC1D23",
  "model": "Civic",
  "entryTime": "2026-02-14T10:00:00",
  "exitTime": "2026-02-14T13:30:00",
  "timeSpent": "03:30",
  "totalPrice": 25.00,
  "currency": "R$"
}
```
**Response 400:** `"Veículo não encontrado no pátio ou já saiu."`

---

#### `GET /api/SessionParking` — Listar todas as sessões ativas (veículos no pátio)
**Response 200:**
```json
[
  {
    "plate": "ABC1D23",
    "model": "Civic",
    "color": "Preto",
    "entryTime": "2026-02-14T10:00:00"
  }
]
```

---

#### `GET /api/SessionParking/checkout/preview/{plate}` — Preview do checkout (sem finalizar)
**URL Param:** `plate` (string) — placa do veículo
**Response 200:**
```json
{
  "plate": "ABC1D23",
  "model": "Civic",
  "entryTime": "2026-02-14T10:00:00",
  "exitTime": "2026-02-14T13:30:00",
  "timeSpent": "03:30",
  "totalPrice": 25.00,
  "currency": "R$"
}
```
**Response 400:** `"Veículo não encontrado no pátio ou já saiu."`

---

### 4. Report (Relatórios / Dashboards)

#### `GET /api/Report/revenue?days={7|30}` — Faturamento por dia
**Query Param:** `days` (int) — obrigatório, aceita apenas `7` ou `30`
**Response 200:**
```json
{
  "items": [
    {
      "date": "2026-02-08",
      "totalSessions": 12,
      "revenue": 180.00
    },
    {
      "date": "2026-02-09",
      "totalSessions": 8,
      "revenue": 120.00
    }
  ],
  "totalRevenue": 300.00
}
```
**Response 400:** `"Informe 7 ou 30 para o período de dias."`

> **Lógica:** Agrupa sessões finalizadas pela data de saída (`exitTime`), soma o `amountCharged` de cada dia. Retorna em ordem crescente de data.

---

#### `GET /api/Report/top-vehicles?start={datetime}&end={datetime}` — Top 10 veículos por tempo estacionado
**Query Params:**
- `start` (DateTime, formato ISO) — ex: `2026-02-01T00:00:00`
- `end` (DateTime, formato ISO) — ex: `2026-02-14T23:59:59`

**Response 200:**
```json
{
  "items": [
    {
      "plate": "ABC1D23",
      "model": "Civic",
      "totalSessions": 5,
      "totalTimeParked": "48:30:00",
      "totalMinutes": 2910.00
    },
    {
      "plate": "XYZ9A87",
      "model": "Corolla",
      "totalSessions": 3,
      "totalTimeParked": "36:15:00",
      "totalMinutes": 2175.00
    }
  ]
}
```
**Response 400:** `"A data de início deve ser anterior à data de fim."`

> **Lógica:** Busca sessões ativas e finalizadas que se sobrepõem ao período. Calcula o tempo efetivo dentro do intervalo (clipa entrada/saída nos limites do período). Agrupa por veículo, soma tempo total, ordena decrescente e retorna os 10 primeiros. `totalTimeParked` usa o formato `HH:mm:ss` (horas podem ultrapassar 24).

---

#### `GET /api/Report/occupancy?start={datetime}&end={datetime}` — Ocupação por hora do dia
**Query Params:**
- `start` (DateTime, formato ISO) — ex: `2026-02-01T00:00:00`
- `end` (DateTime, formato ISO) — ex: `2026-02-14T23:59:59`

**Response 200:**
```json
{
  "items": [
    {
      "hour": 0,
      "hourRange": "00:00 - 01:00",
      "averageVehicles": 2.5,
      "maxVehicles": 5
    },
    {
      "hour": 1,
      "hourRange": "01:00 - 02:00",
      "averageVehicles": 1.8,
      "maxVehicles": 3
    },
    {
      "hour": 8,
      "hourRange": "08:00 - 09:00",
      "averageVehicles": 15.3,
      "maxVehicles": 22
    },
    {
      "hour": 12,
      "hourRange": "12:00 - 13:00",
      "averageVehicles": 20.1,
      "maxVehicles": 28
    }
  ]
}
```
**Response 400:** `"A data de início deve ser anterior à data de fim."`

> **Lógica:** Para cada hora do dia (0–23), percorre cada dia do período e conta quantas sessões estavam ativas naquele slot de 1 hora. Retorna a **média** (quantos veículos estavam no pátio em média naquela hora) e o **máximo** (pico). Retorna até 24 itens (um por hora). Ideal para gráfico de barras ou área.

---

## Resumo de Rotas (tabela rápida)

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/Parking` | Criar pátio |
| GET | `/api/Parking` | Listar pátios |
| POST | `/api/Vehicle` | Cadastrar veículo |
| PUT | `/api/Vehicle/{plate}` | Atualizar veículo por placa |
| GET | `/api/Vehicle` | Listar veículos |
| POST | `/api/SessionParking/checkin` | Check-in (entrada) |
| POST | `/api/SessionParking/checkout` | Check-out (saída + cobrança) |
| GET | `/api/SessionParking` | Listar sessões ativas |
| GET | `/api/SessionParking/checkout/preview/{plate}` | Preview do checkout |
| GET | `/api/Report/revenue?days=7` | Faturamento por dia |
| GET | `/api/Report/top-vehicles?start=...&end=...` | Top 10 veículos |
| GET | `/api/Report/occupancy?start=...&end=...` | Ocupação por hora |

---

## Tratamento de Erros

Todos os endpoints seguem o mesmo padrão:
- **Sucesso:** Retorna `200 OK`, `201 Created` ou `204 NoContent`
- **Erro de negócio:** Retorna `400 Bad Request` com a mensagem de erro no body como **string pura** (não é JSON com campo `message`, é a string diretamente)

Exemplo de erro:
```
HTTP 400
"Veículo não encontrado no pátio ou já saiu."
```

---

## Sugestão de Telas para o Front-End Angular

### 1. Dashboard Principal
- **Card:** Total de veículos no pátio agora (GET `/api/SessionParking` → contar itens)
- **Card:** Faturamento dos últimos 7 dias (GET `/api/Report/revenue?days=7` → `totalRevenue`)
- **Gráfico de barras:** Faturamento por dia nos últimos 7 dias (`items[].date` x `items[].revenue`)
- **Gráfico de área/barras:** Ocupação por hora do dia (GET `/api/Report/occupancy` → `items[].hourRange` x `items[].averageVehicles`)

### 2. Tela de Check-in
- Formulário com campos: `plate`, `model`, `color`, `type`
- Botão "Registrar Entrada" → POST `/api/SessionParking/checkin`
- Feedback de sucesso ou erro

### 3. Tela de Check-out
- Campo de busca por placa
- Botão "Consultar" → GET `/api/SessionParking/checkout/preview/{plate}` (mostra preview com tempo e valor)
- Botão "Confirmar Saída" → POST `/api/SessionParking/checkout`
- Exibe recibo com: placa, modelo, entrada, saída, tempo, valor total, moeda

### 4. Tela de Veículos no Pátio (Sessões Ativas)
- Tabela com dados de GET `/api/SessionParking`
- Colunas: Placa, Modelo, Cor, Hora de Entrada
- Botão "Checkout" em cada linha que redireciona para a tela de checkout com a placa preenchida

### 5. Tela de Relatórios
- **Aba "Faturamento":**
  - Toggle para 7 ou 30 dias
  - Tabela: Data | Sessões | Receita
  - Gráfico de barras com a receita por dia
  - Rodapé com total geral

- **Aba "Top 10 Veículos":**
  - Date range picker (start/end)
  - Tabela: Posição | Placa | Modelo | Sessões | Tempo Total | Minutos
  - Gráfico de barras horizontal

- **Aba "Ocupação por Hora":**
  - Date range picker (start/end)
  - Gráfico de área: hora do dia (eixo X) x média de veículos (eixo Y)
  - Linha pontilhada para o máximo
  - Tabela complementar: Hora | Faixa | Média | Máximo

### 6. Tela de Gestão de Veículos
- Tabela com GET `/api/Vehicle`
- Colunas: ID, Placa, Modelo, Cor, Tipo
- Botão "Novo Veículo" → modal/formulário → POST `/api/Vehicle`
- Botão "Editar" em cada linha → modal/formulário → PUT `/api/Vehicle/{plate}`

### 7. Tela de Gestão de Pátios
- Tabela com GET `/api/Parking`
- Colunas: Código, Cidade
- Botão "Novo Pátio" → formulário → POST `/api/Parking`

---

## Stack recomendada para o Front-End

- **Angular 19+** com standalone components
- **Angular Material** ou **PrimeNG** para componentes UI
- **ngx-charts** ou **Chart.js** (via ng2-charts) para gráficos nos dashboards
- **HttpClient** para consumir a API
- **Angular Router** para navegação entre as telas
- **Reactive Forms** para formulários com validação

---

## Como rodar o Backend

```bash
cd HostwayParking
dotnet run
```

A API estará disponível em:
- HTTP: `http://localhost:5217`
- HTTPS: `https://localhost:7062`
- Scalar (documentação interativa): `https://localhost:7062/scalar/v1`
- OpenAPI spec: `https://localhost:7062/openapi/v1.json`