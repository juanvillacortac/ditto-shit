{{ $namespace := NodeOption .Root "cs_namespace" -}}
{{- $filter := NodeOption .Model "filter" -}}
using AutoMapper;

{{- if $filter }}{{with Model $filter}}{{if not (NodeOption . "embedFilter")}}
using {{ $namespace }}.Domain.{{ .Name | CamelCase }}Domain;
{{- end }}{{ end }}{{ end }}
using {{ $namespace }}.Domain.{{ .Model.Name | CamelCase }}Domain;
{{- range $key, $dep := ModelDeps .Model }}
using {{ $namespace }}.Domain.{{ $dep.Name | CamelCase }}Domain;
{{- end }}

{{- if $filter }}{{with Model $filter}}{{if not (NodeOption . "embedFilter")}}
using {{ $namespace }}.API.ViewModels.{{ .Name | CamelCase }};
{{- end }}{{ end }}{{ end }}
using {{ $namespace }}.API.ViewModels.{{ .Model.Name | CamelCase }};
{{- range $key, $dep := ModelDeps .Model }}
using {{ $namespace }}.API.ViewModels.{{ $dep.Name | CamelCase }};
{{- end }}

namespace {{ $namespace }}.API.AutoMapper
{
    public class {{ .Model.Name | CamelCase }}Profile : Profile
    {
        public {{ .Model.Name | CamelCase }}Profile()
        {
            {{- range $key, $dep := ModelDeps .Model }}
            _ = CreateMap<{{ $dep.Name | CamelCase }}, {{ $dep.Name | CamelCase }}ViewModel>()
                {{- range $dep.Props }}
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
            {{ range $key, $dep := ModelDeps (Model $filter) }}
            _ = CreateMap<{{ $dep.Name | CamelCase }}, {{ $dep.Name | CamelCase }}ViewModel>()
                {{- range $dep.Props }}
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

            {{- if $filter }}
            {{- with Model $filter }}
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
            {{- end }}

            _ = CreateMap<{{ .Model.Name | CamelCase }}, {{ .Model.Name | CamelCase }}ViewModel>()
                {{- range .Model.Props }}
                {{- $another := Model .Type -}}
                {{- if $another }}
                .ForMember(
                    dest => dest.{{ .Name | CamelCase }},
                    opt => opt.MapFrom(src => src.{{ .Name | CamelCase }})
                )
                {{- end }}
                {{- end }}
                .ReverseMap();
        }
    }
}