# Introduction
Tool that enables you to use local file system template links

# Currently supported:
- Use fully qualified  or relative file:// links in the templateLink url
- Supports forward slash directory separator
- Currently the expanded ARM template is stored with adding '.expanded' between the file name and extension.
	Example: C:\template.expanded.json
	If the file already exists, it will be owerriden. 

# Planned features (roadmap):
- Expand nested templates
- Option to save nested templates to filesystem also
- Use http:// links
- Support for parameterLink expansion
- Detect endless loops

# Not supported or planned features
- Any template validation is left to Azure until the point a public library is available to do that.
	The templates are treated as json. 
	They are considered valid as long as the json is valid.
