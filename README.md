# 19N11 - TEAM 6 - Parking: Parking Utility For Small Business
- Link video demo:
  https://drive.google.com/drive/folders/1HquSLd_xL3HNTpYqaBb71FTwswcGT8Rn?usp=sharing
- Link slide: 
  https://www.canva.com/design/DAFUEGhkDbs/sb1MqCyQrUKaNkFc2aRbhw/edit?utm_content=DAFUEGhkDbs&utm_campaign=designshare&utm_medium=link2&utm_source=sharebutton
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
