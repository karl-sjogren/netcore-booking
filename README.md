# Configuration

The following user secrets should be set before starting the application. In production these should be environment variables.

* MONGODB_URI
* SENDGRID_APIKEY
* SENDGRID_FROM
* AUTH_PIN
* AUTH_USER
* AUTH_PASSWORD

```
dotnet user-secrets set MONGODB_URI mongodb://user:pass@host.tld/database
```