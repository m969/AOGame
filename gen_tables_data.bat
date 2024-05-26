set WORKSPACE=.\

set GEN_CLIENT=%WORKSPACE%\AOTools\_ToolsBin\Luban.ClientServer\Luban.ClientServer.exe
set CONF_ROOT=%WORKSPACE%\ExcelTables

%GEN_CLIENT% -j cfg --^
 -d %CONF_ROOT%\Defines\__root__.xml ^
 --input_data_dir %CONF_ROOT% ^
 --output_code_dir AOClient/Unity/Assets/Game.Model/_AutoGenerates/TablesModel ^
 --output_data_dir AOClient/Unity/Assets/Bundles/ExcelTablesData ^
 --gen_types code_cs_unity_json,data_json ^
 -s all 

%GEN_CLIENT% -j cfg --^
 -d %CONF_ROOT%\Defines\__root__.xml ^
 --input_data_dir %CONF_ROOT% ^
 --output_code_dir AOServer/Service.Model/_AutoGenerates/TablesModel ^
 --output_data_dir ./ExcelTablesData ^
 --gen_types code_cs_dotnet_json,data_json ^
 -s all 

pause