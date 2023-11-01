# AddressStandardizer

Web-service that standardize raw address using Dadata.

# Setup

Put Dadata api `Key` and `Secret` to corresponding field in `appsettings.json` or use user secrets managenent.

# Usage

```
curl -X GET \
   -H 'Content-Type: application/json'
   -d '{"address": "Воронеж Плехановская 10"}'
   https://localhost:7167/api/address/clean
```

Service will return json from Dadata.

# Error handling

On any errors occured on Dadata api side, service will return 400 BadRequest. Error details will be logged.
