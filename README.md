# Introduction
Generate expanded ARM templates from templates with template links from local file system.  Simply upload the expanded template to Azure without the need of also uploading your nested templates. Enables you to test ARM templates which contain template links by using tools like Pester.

# Currently supported:
- Use fully qualified  or relative file:// links in the templateLink url
- Supports forward slash directory separator
- Expand nested templates
- Detect endless loops
- Support for parameterLink expansion
- Currently the expanded ARM template is stored with adding '.expanded' between the file name and extension.
	Example: C:\template.expanded.json
	If the file already exists, it will be owerriden. 
- Option to set the expanded file name

# Planned features (roadmap):
- Option to save nested templates to filesystem while expanding the parent
- Support for https:// links

# Not supported or planned features
- Any template validation is left to Azure until the point a public library is available to do that.
	The templates are treated as json. 
	They are considered valid as long as the json is valid.

# Usages
- minimal:
``` console
  ExpandARM.exe -i "complex-template.json"
```	
- full:
``` console
  ExpandARM.exe -i "complex-template.json" -o "expanded-complex-template.json" -v
```	

### build [![Build Status](https://borisjuraga.visualstudio.com/ExpandARM/_apis/build/status/ExpandARM-CI?branchName=master)](https://borisjuraga.visualstudio.com/ExpandARM/_build/latest?definitionId=12?branchName=master)

