{{- $filter := NodeOption .Model "filter" -}}
{{ $namespace := NodeOption .Root "cs_namespace" -}}
namespace {{ $namespace }}.Domain.{{ .Model.Name | CamelCase }}Domain
{
    public class {{ .Model.Name | CamelCase }}Service : I{{ .Model.Name | CamelCase }}Service
    {
        private readonly I{{ .Model.Name | CamelCase }}Repository _repository;

        public {{ .Model.Name | CamelCase }}Service(I{{ .Model.Name | CamelCase }}Repository repository) =>
            this._repository = repository;
        {{ if and $filter (not (NodeOption .Model "singular")) }}
        public System.Collections.Generic.List<{{ .Model.Name | Plural | CamelCase }}> Get{{ .Model.Name | Plural }}List({{ $filter | CamelCase }} filter) =>
            this._repository.Get{{ .Model.Name | Plural }}List(filter);
        {{ end }}

        {{- if NodeOption .Model "singularFromFilter" }}
        public {{ .Model.Name | CamelCase }} Get{{ .Model.Name | CamelCase }}({{ $filter | CamelCase }} filter) =>
            this._repository.Get{{ .Model.Name | CamelCase }}(filter);
        {{- else }}
        {{- $pk := .Model.PKProp }}
        {{- if $pk }}
        public {{ .Model.Name | CamelCase }} Get{{ .Model.Name | CamelCase }}({{ $pk.Type }} {{ $pk.Name }}) =>
            this._repository.Get{{ .Model.Name | CamelCase }}($pk.Name);
        {{- end }}
        {{- end }}
        {{- if not (NodeOption .Model "notPost") }}
        public long Post{{ .Model.Name | CamelCase }}(model: {{ .Model.Name }}, long idUser, long idBusiness) =>
            this._repository.Post{{ .Model.Name | CamelCase }}(model, idUser, idBusiness);
        {{- end }}
    }
}
