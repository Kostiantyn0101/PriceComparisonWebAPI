# PriceComparison Web API

**Backend service for a full‑stack price‑ & product‑comparison platform built with Clean Architecture (Domain → DLL → BLL → API).**

---

## ✨ Key Features

| Module     | Highlights                                                                                                  |
| ---------- | ----------------------------------------------------------------------------------------------------------- |
| **Domain** | Pure C# entities, value objects & interfaces, free of any framework dependencies.                           |
| **DLL**    | Entity Framework Core data layer, fluent mappings & migrations, repository abstractions.                    |
| **BLL**    | Business services for categories, products, sellers, prices, media, filters, AI comparison & more.          |
| **API**    | ASP.NET Core Web API with JWT auth (user / seller / manager roles), Swagger‑UI & global exception handling. |

Additional capabilities:

* **AI‑powered comparison** – integrates both *OpenAI* & *Anthropic* (Claude) to generate human‑readable product comparisons.
* **Media management** – endpoints for images, videos, manuals & feedback content.
* **Dynamic filtering** – server‑side filtering by any characteristic, plus auction‑based price ranking.
* **Price history** – tracks daily price snapshots from multiple sellers.
* **Docker‑ready** – production Dockerfile included in *PriceComparisonWebAPI/*.

> **Note:** Unit‑tests are not yet included. Adding integration & service‑level tests is planned in the roadmap.

---

## 🛠️ Tech Stack

* **.NET 8**, C# 12
* **ASP.NET Core** Web API
* **Entity Framework Core** 8
* **FluentValidation**, **AutoMapper**
* **JWT Bearer** authentication & authorization
* **Swagger / Swashbuckle** for live API docs
* **Docker** (Multi‑stage build)

---

## 📂 Repository Layout

```
├── Domain/                 # Entities & primitives
├── DLL/                    # EF Core context, configs & migrations
├── BLL/
│   ├── AIServices/         # OpenAI / Anthropic integration
│   ├── Auth/               # Auth helpers
│   ├── CategoryServices/
│   ├── …                   # Feedback, Filter, Media, Price, Product, Seller
├── PriceComparisonWebAPI/  # API layer (controllers, DI, middleware, Dockerfile)
└── PriceComparisonWebAPI.sln
```

---

## 🚀 Getting Started

### Prerequisites

* **.NET 8 SDK**
* **SQL Server** (localdb is fine)

### 1. Clone & restore

```bash
git clone https://github.com/Kostiantyn0101/PriceComparisonWebAPI.git
cd PriceComparison-Web-API
dotnet restore
```

### 2. Configure secrets

Create **PriceComparisonWebAPI/appsettings.Secrets.json** (this file is excluded via *.gitignore*).

> 🔐 **Mandatory:** the API will not start without all three of these secrets:
>
> * **ConnectionStrings\:DefaultConnection** – SQL Server connection string
> * **Jwt\:Key** – symmetric key to sign JWT access tokens
> * **OpenAI\:ApiKey** – enables GPT‑powered comparison features
>
> *(Anthropic key is optional – only needed if you plan to use Claude‑based comparisons).*
>
> You can store secrets locally with `dotnet user-secrets` or as environment variables in production.

```jsonc
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=PriceComparison;Trusted_Connection=True;"
  },
  "Jwt": {
    "Key": "<super-secret-key>",
    "Issuer": "PriceComparison"
  },
  "OpenAI": {
    "ApiKey": "<your-openai-key>"
  },
  "Anthropic": {
    "ApiKey": "<your-claude-key>",
    "ModelName": "claude-3-7-sonnet-20250219"
  }
}
```

### 3. Apply migrations

```bash
dotnet ef database update --project DLL --startup-project PriceComparisonWebAPI
```

### 4. Run

```bash
dotnet run --project PriceComparisonWebAPI
# Swagger UI → https://localhost:7009/swagger/index.html
```

### 5. Docker (optional)

```bash
docker build -t pricecomparison-api -f PriceComparisonWebAPI/Dockerfile .
docker run -p 8080:8080 -e ConnectionStrings__DefaultConnection="..." pricecomparison-api
```

---

## 📜 Seed Database (optional)

> **This step is *optional*.** You can rely entirely on EF Core migrations. The SQL script is just a convenience when you want demo data instantly.

A quick‑start SQL script with schema **and sample data** is available in database/PriceComparison_init.sql_+data.sql.

1. Ensure your target database (**PriceComparison**) exists or adjust the first `CREATE DATABASE` line.

2. Run the script via Management Studio or the command line:

   ```bash
   sqlcmd -S (localdb)\mssqllocaldb -i database/PriceComparison_init.sql_+data.sql
   ```

3. **Skip step 3** in *Getting Started* (migrations) because the tables & seed data are already in place. SQL script with schema **and sample data** is available in database/PriceComparison_init.sql_+data.sql.

4. Ensure your target database (**PriceComparison**) exists or adjust the first `CREATE DATABASE` line.

5. Run the script via Management Studio or the command line:

   ```bash
   sqlcmd -S (localdb)\mssqllocaldb -i database/PriceComparison_init.sql_+data.sql
   ```

6. Skip step **3** in *Getting Started* (migrations) because the tables & seed data are already in place.

---

## 🗺️ Roadmap

* [ ] **Unit & integration tests** (xUnit + Testcontainers)
* [ ] **Rate‑limited seller API** for price ingestion
* [ ] **Background jobs** (Hangfire) for price snapshots
* [ ] **GraphQL** endpoint for front‑end flexibility
* [ ] **Observability** (Serilog + OpenTelemetry)

---

## 🔗 Related repositories

* **Backend API** – [Kostiantyn0101/PriceComparisonWebAPI](https://github.com/Kostiantyn0101/PriceComparisonWebAPI)
* **Admin Panel** – [Kostiantyn0101/PriceComparison-UI-MVC-admin](https://github.com/Kostiantyn0101/PriceComparison-UI-MVC-admin)
* **PriceComparison MVC Front‑end** – [Kostiantyn0101/PriceComparison-UI-MVC](https://github.com/Kostiantyn0101/PriceComparison-UI-MVC)

---

## 🤝 Contributing

Pull‑requests are welcome! Please open an issue first to discuss major changes.

---

## 📝 License

This project is licensed under the **MIT License**
