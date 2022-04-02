{{- define "props"}}
{{- range $i, $p := .Props}}{{ if $i }},{{ end }}
{{- $another := Model $p.Type -}}
{{- if $another }}
"{{ NodeOptionOr $p "alt" ($p.Name | CamelCase) }}": {{ if $p.IsArray }}[{{ else }}{{ "{" }}{{end}}
    {{- ExecTmpl "props" $another | indent 4 }}
{{ if .IsArray }}]{{ else }}{{ "}" }}{{ end }}
{{- else }}
"{{ NodeOptionOr $p "alt" ($p.Name | CamelCase) }}": {{ if $p.IsArray }}[{{$p.Type}}]{{ else }}"{{$p.Type}}"{{end}}
{{- end }}
{{- end }}
{{- end }}
{{- $filter := NodeOption .Model "filter" -}}
{{ if not (NodeOption .Model "singular") }}
## Obtener listado de `{{ NodeOption .Model "alt" }}`

{{- if $filter }}
### Filtro
```json
{
    {{- ExecTmpl "props" $filter | indent 4 }}
}
```
{{- end }}

### Respuesta
```json
[
    {
        {{- ExecTmpl "props" .Model | indent 8 }}
    }
]
```
{{ else }}
## Obtener `{{ NodeOption .Model "alt" }}`

```json
{
    {{- ExecTmpl "props" .Model | indent 4 }}
}
```
{{ end }}