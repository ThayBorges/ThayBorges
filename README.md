# ♻️ ESG Waste Management API

Aplicação ASP.NET Core 8 focada no tema **gestão de resíduos e reciclagem**, desenvolvida para o desafio de criação de endpoints RESTful robustos seguindo boas práticas de MVVM, paginação, segurança e testes automatizados.

## ✨ Principais funcionalidades
- **4 controllers dedicados ao domínio** (`CollectionPoints`, `CollectionRequests`, `Alerts`, `ImpactReports`) + `AuthController` para geração de tokens JWT.
- **Paginação obrigatória** em todos os endpoints de listagem, garantindo escalabilidade.
- **Autenticação e autorização** com JWT + policies específicas (`PlannerOnly`, `AuditorOnly`) para operações críticas.
- **Validações com FluentValidation** e middleware global de tratamento de exceções com respostas em `ProblemDetails`.
- **Integração com banco SQLite** (migrations inclusas) e `DatabaseSeeder` com dados ESG realistas.
- **Arquitetura em camadas (MVVM-inspired)**: Domain (modelos), Application (ViewModels, DTOs, validators), Infrastructure (EF Core, serviços), Api (controllers/middleware).
- **Testes de integração com xUnit + WebApplicationFactory**, garantindo status `200` para cada controller.
- **Dockerfile** pronto para deploy e coleção Postman/Insomnia para facilitar a correção.

## 🗂️ Estrutura do projeto
```
/ src
  ├─ WasteManagement.Domain               # Entidades e enums
  ├─ WasteManagement.Application          # DTOs, view models, validators, interfaces
  ├─ WasteManagement.Infrastructure       # EF Core, serviços, migrations, seed
  └─ WasteManagement.Api                  # Controllers, middleware, Program.cs
/ tests
  └─ WasteManagement.Tests                # Testes xUnit (status code 200)
/dist | /postman                          # Artefatos de entrega (zip + coleção)
```

## ⚙️ Requisitos
- .NET 8 SDK
- SQLite (utiliza arquivo `data/waste.db`, criado automaticamente)
- Opcional: Docker (para build e execução containerizada)

## ▶️ Como executar
```bash
# Restaurar dependências
 dotnet restore

# Aplicar migrations (gera/atualiza data/waste.db)
 dotnet ef database update -s src/WasteManagement.Api -p src/WasteManagement.Infrastructure

# Executar API
 dotnet run --project src/WasteManagement.Api
```
A API sobe, por padrão, em `http://localhost:5000` (ou `https://localhost:5001`). Swagger disponível em `/swagger`.

### Usuários padrão (appsettings)
| Usuário   | Senha          | Perfil   | Uso principal                       |
|-----------|----------------|----------|-------------------------------------|
| planner   | planner@2024   | Planner  | CRUD de pontos, solicitações, alertas|
| auditor   | auditor@2024   | Auditor  | Consulta de relatórios de impacto   |

Use o endpoint `POST /api/auth/token` para obter o JWT e incluí-lo no header `Authorization: Bearer {token}`.

## ✅ Testes
```bash
dotnet test
```
Os testes usam banco InMemory, seed automático e verificam se cada controller retorna `200` em seu endpoint principal.

## 🐳 Docker
```bash
docker build -t esg-waste-api .
docker run -p 8080:8080 esg-waste-api
```
O container expõe `http://localhost:8080`. O arquivo `data/waste.db` fica dentro do container; monte um volume se quiser persistir.

## 📬 Coleção Insomnia/Postman
- Arquivo: `postman/ESG-Waste-Management.postman_collection.json`
- Versão zipada para entrega: `dist/insomnia-collection.zip`
Inclui exemplos para todos os endpoints (listar, criar, atualizar, gerar token, etc.).

## 📦 Entregáveis solicitados
- `dist/esg-waste-management.zip`: código-fonte completo + Dockerfile + migrations.
- `dist/insomnia-collection.zip`: coleção pronta para importação.

---
Projeto criado com foco em sustentabilidade urbana, demonstrando técnicas avançadas de APIs RESTful em .NET 8 para ambientes avaliativos e prontos para produção. 🚀
