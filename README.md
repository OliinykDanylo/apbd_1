# apbd_test1
To run this application, you must provide an appsettings.json configuration file in the root of your project. The file should include the following sections:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "<YOUR_DATABASE_CONNECTION_STRING>"
  }
}
```

You should replace <YOUR_DATABASE_CONNECTION_STRING> with your actual connection string.