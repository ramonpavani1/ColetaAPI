# ColetaAPI - Deploy Automatizado com GitHub Actions + Azure

Este projeto utiliza integração contínua (CI) e entrega contínua (CD) usando **GitHub Actions** e **Azure Web App** com **OpenID Connect** (OIDC), sem precisar de perfil de publicação ou FTP.

---

## ✅ Tecnologias usadas

- ASP.NET Core (.NET 8)
- GitHub Actions
- Azure Web App (cidadeinteligente-dev / cidadeinteligente-prod)
- Autenticação federada (OIDC)
- Deploy via CI/CD

---

## 🔧 Como rodar localmente

```bash
dotnet restore
dotnet build
dotnet run
