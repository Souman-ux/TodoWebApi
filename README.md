# TodoWebApi – User Login REST API

This ASP.NET Core Web API allows adding users, logging in, checking login status, locking/unlocking accounts, and listing users.  
Data is stored in memory and resets when the server restarts.

---

## 🔧 Endpoints

### ➕ Add User

`POST /User`

```json
{
  "userName": "alice",
  "password": "password123"
}
```
