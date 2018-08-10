# Introduction
Tool that enables you to use local file system template links

# Currently supported:
- Use fully qualified  or relative file:// links in the templateLink url
- Supports forward slash directory separator
- Expand nested templates
- Currently the expanded ARM template is stored with adding '.expanded' between the file name and extension.
	Example: C:\template.expanded.json
	If the file already exists, it will be owerriden. 

# Planned features (roadmap):
- Option to save nested templates to filesystem while expanding the parent
- Use https:// links
- Support for parameterLink expansion
- Detect endless loops

# Not supported or planned features
- Any template validation is left to Azure until the point a public library is available to do that.
	The templates are treated as json. 
	They are considered valid as long as the json is valid.

# Usage
	
	ExpandARM.exe -i "path\to\arm template"

### build [![Build status](https://borisjuraga.visualstudio.com/ExpandARM/_apis/build/status/ExpandARM_CI)](https://borisjuraga.visualstudio.com/ExpandARM/_build/latest?definitionId=7)
### release [![Build status](https://borisjuraga.visualstudio.com/ExpandARM/_apis/build/status/ExpandARM_Release)](https://borisjuraga.visualstudio.com/ExpandARM/_build/latest?definitionId=8)