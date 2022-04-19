{{- define "block" }}
{{- range $key, $dep := ModelDeps . -}}
{{- template "block" $dep }}
{{- end }}
_ = CreateMap<{{ .Name | CamelCase }}, {{ .Name | CamelCase }}ViewModel>()
    {{- range .Props }}
    {{- $another := Model .Type -}}
    {{- if $another }}
    .ForMember(
        dest => dest.{{ .Name | CamelCase }},
        opt => opt.MapFrom(src => src.{{ .Name | CamelCase }})
    )
    {{- end }}
    {{- end }}
    .ReverseMap();
{{- end }}
{{ $namespace := NodeOption .Root "cs_namespace" -}}
{{- $filter := NodeOption .Model "filter" -}}
using AutoMapper;

{{- if $filter }}{{with Model $filter}}{{if not (NodeOption . "embedFilter")}}
using {{ $namespace }}.Domain.{{ .Name | CamelCase }}Domain;
{{- end }}{{ end }}{{ end }}
using {{ $namespace }}.Domain.{{ .Model.Name | CamelCase }}Domain;
{{- range $key, $dep := ModelDeps .Model }}
{{- range $key, $dep2 := ModelDeps $dep }}
using {{ $namespace }}.Domain.{{ $dep2.Name | CamelCase }}Domain;
{{- end }}
using {{ $namespace }}.Domain.{{ $dep.Name | CamelCase }}Domain;
{{- end }}

{{- if $filter }}{{with Model $filter}}{{if not (NodeOption . "embedFilter")}}
using {{ $namespace }}.API.ViewModels.{{ .Name | CamelCase }};
{{- end }}{{ end }}{{ end }}
using {{ $namespace }}.API.ViewModels.{{ .Model.Name | CamelCase }};
{{- range $key, $dep := ModelDeps .Model }}
{{- range $key, $dep2 := ModelDeps $dep }}
using {{ $namespace }}.API.ViewModels.{{ $dep2.Name | CamelCase }};
{{- end }}
using {{ $namespace }}.API.ViewModels.{{ $dep.Name | CamelCase }};
{{- end }}

namespace {{ $namespace }}.API.AutoMapper
{
    public class {{ .Model.Name | CamelCase }}Profile : Profile
    {
        public {{ .Model.Name | CamelCase }}Profile()
        {
            {{- if $filter }}
            {{- with Model $filter }}{{ (ExecTmpl "block" .) | indent 12 }}{{ end }}
            {{- end }}
            {{- (ExecTmpl "block" .Model) | indent 12 }}
        }
    }
}