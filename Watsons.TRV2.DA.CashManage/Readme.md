1.Make sure solution is build successful.

2.Open Package Manager Console, run command below:
(a)CashManage Database
`Scaffold-DbContext 'Server=10.98.32.248;Database=CashManage;User ID=sa;Password=!QAZ2wsx#EDC;TrustServerCertificate=true;' Microsoft.EntityFrameworkCore.SqlServer -Force -Context CashManageContext -OutputDir Entities -ContextDir Entities -Namespace Watsons.TRV2.DA.CashManage.Entities -ContextNamespace Watsons.TRV2.DA.CashManage.Entities -Tables UserLogin, UserStoreId -Project Watsons.TRV2.DA.CashManage -StartupProject Watsons.TRV2.DA.CashManage`

3.Remove OnConfiguring from <project_name>Context.cs

4. NativeAOT project must compile model before use it.
`dotnet ef dbcontext optimize --output-dir MyDbContextModel --context MyDbContext --namespace MyProject.Data`