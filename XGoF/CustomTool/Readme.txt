INSTALLATION:
1 - Compile the project
2 - Open command prompt in the bin folder
3 - Type: regasm /codebase NMatrix.XGoF.CustomTool.dll

Notes: 
1 - To unregister: regasm /unregister NMatrix.XGoF.CustomTool.dll
2 - A recompilation of the tool requires unregistration/re-registration of the library
3 - Associate the custom tool with the configuration file througth the property browser. Set custom tool property to XGoF and build to Content. If more than one output file results from the generation, they are saved to the specified target location (just like the add-in and console version do) but additionally they are put together in a single file depending on the configuration file. Would it be better to only output the single file?
