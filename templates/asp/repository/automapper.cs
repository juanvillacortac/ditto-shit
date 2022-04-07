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
using {{ $namespace }}.Repository.Dao.{{ .Name | CamelCase }};
{{- end }}{{ end }}{{ end }}
using {{ $namespace }}.Repository.Dao.{{ .Model.Name | CamelCase }};
{{- range $key, $dep := ModelDeps .Model }}
using {{ $namespace }}.Repository.Dao.{{ $dep.Name | CamelCase }};
{{- end }}

namespace {{ $namespace }}.Repository.AutoMapper
{
    public class {{ .Model.Name | CamelCase }}RepositoryProfile : Profile
    {
        public {{ .Model.Name | CamelCase }}RepositoryProfile()
        {
            {{- range $key, $dep := ModelDeps .Model }}
            _ = CreateMap<{{ $dep.Name | CamelCase }}, {{ $dep.Name | CamelCase }}Dao>()
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
            _ = CreateMap<{{ $dep.Name | CamelCase }}, {{ $dep.Name | CamelCase }}Dao>()
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
            _ = CreateMap<{{ .Name | CamelCase }}, {{ .Name | CamelCase }}Dao>()
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

            _ = CreateMap<{{ .Model.Name | CamelCase }}, {{ .Model.Name | CamelCase }}Dao>()
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