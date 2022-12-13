# DUTPS-Backend
- Use git clone to clone repository
- config appsettings.Development.json file on ConnectionStrings
- Open command line
  + dotnet restore
  + cd DUTPS.Databases
  + dotnet-ef database update --startup-project ../DUTPS.API/
  + cd into DUTPS.API folder
  + dotnet build
  + dotnet watch run
- Super user
  + Username: admin
  + Password: Pa$$w0rd
- Link Video demo:
https://drive.google.com/drive/folders/1HquSLd_xL3HNTpYqaBb71FTwswcGT8Rn?usp=sharing