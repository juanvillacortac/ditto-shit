USE [{{ NodeOption .Root "database_name" }}]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
{{ $modelAlt := NodeOptionOr .Model "alt" (.Model.Name | CamelCase) -}}
CREATE TABLE [{{ NodeOption .Root "schema_name" }}].[{{ $modelAlt }}](
    {{ range .Model.Props }}{{ if not .IsArray -}}
    {{ $propAlt := NodeOption . "alt" -}}
    {{- if not $propAlt -}}
    {{- $propAlt = .Name | CamelCase -}}
    {{- end -}}
    [{{ $propAlt }}] [{{ .Type }}]{{ if .PK }} IDENTITY(1, 1){{ end }}{{ if .IsRequired }} NOT NULL{{ end }},
    {{ end }}{{- end }}
 CONSTRAINT [PK_{{ $modelAlt }}] PRIMARY KEY CLUSTERED 
(
	[Id{{ $modelAlt }}] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

{{- range .Model.Props }}{{ if .DefaultValue -}}
{{ $propAlt := NodeOption . "alt" -}}
{{- if not $propAlt -}}
{{- $propAlt = .Name | CamelCase -}}
{{- end }}
ALTER TABLE [Maestro].[{{ $modelAlt }}] ADD CONSTRAINT [DF_{{ $modelAlt }}_{{ $propAlt }}] DEFAULT ({{ .DefaultValue }}) FOR [{{ $propAlt }}]
GO
{{ end }}{{- end }}