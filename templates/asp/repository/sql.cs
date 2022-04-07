{{ $namespace := NodeOption .Root "cs_namespace" -}}
{{- $filter := NodeOption .Model "filter" -}}
using AutoMapper;


{{- if $filter }}{{with Model $filter}}{{if not (NodeOption . "embedFilter")}}
using {{ $namespace }}.Domain.{{ .Name | CamelCase }}Domain;
{{- end }}{{ end }}{{ end }}
using {{ $namespace }}.Domain.{{ .Model.Name | CamelCase }}Domain;
using {{ $namespace }}.Repository.Dao.{{ .Model.Name | CamelCase }};
{{- if $filter }}{{with Model $filter}}{{if not (NodeOption . "embedFilter")}}
using {{ $namespace }}.Repository.Dao.{{ .Name | CamelCase }};
{{- end }}{{ end }}{{ end }}

using {{ $namespace }}.Repository.Dao.Common;
using {{ $namespace }}.Repository.Interfaces;
using {{ $namespace }}.Repository.Utils;

using Newtonsoft.Json.Linq;

using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace {{ $namespace }}.Repository.Implementations.SqlServer
{
   public class Sql{{ .Model.Name | CamelCase }}Repository : I{{ .Model.Name | CamelCase }}Repository
    {
        private readonly IMapper _mapper;
        private readonly IConnector _connector;

        public Sql{{ .Model.Name | CamelCase }}Repository(IMapper map, IConnector con)
        {
            this._mapper = map;
            this._connector = con;
        }
        {{- if and $filter (not (NodeOption .Model "singular")) }}

        public List<{{ .Model.Name | CamelCase }}> Get{{ .Model.Name | Plural }}List({{ $filter | CamelCase }} filter)
        {
            {{ $filter | CamelCase }}Dao daoFilter = this._mapper.Map<{{ $filter | CamelCase }}Dao>(filter);

            string data = this._connector.GetJson(
              "{{ NodeOption .Model "spGetList" }}",
                JObject.FromObject(this._mapper.Map<{{ $filter | CamelCase }}Dao>(filter))
            );

            List<{{ .Model.Name | CamelCase }}Dao> dao = JsonUtils.DeserializeObjectOrDefault(
                data,
                new List<{{ .Model.Name | CamelCase }}Dao>()
            );

            return this._mapper.Map<List<{{ .Model.Name | CamelCase }}>>(dao)
                .ToList();
        }
        {{- end }}
        {{- if NodeOption .Model "singularFromFilter" }}

        public {{ .Model.Name | CamelCase }} Get{{ .Model.Name }}({{ $filter | CamelCase }} filter)
        {
            {{ $filter | CamelCase }}Dao daoFilter = this._mapper.Map<{{ $filter | CamelCase }}Dao>(filter);

            string data = this._connector.GetJson(
              "{{ NodeOption .Model "spGet" }}",
                JObject.FromObject(this._mapper.Map<{{ $filter | CamelCase }}Dao>(filter))
            );

            {{ if not (NodeOption .Model "isPage") -}}
            List<{{ end }}{{ .Model.Name | CamelCase }}Dao
            {{- if not (NodeOption .Model "isPage") }}>{{ end }} dao = JsonUtils.DeserializeObjectOrDefault(
                data,
                new {{ if not (NodeOption .Model "isPage") }}List<{{- end }}{{ .Model.Name | CamelCase }}Dao{{- if not (NodeOption .Model "isPage") }}>{{ end }}()
            );
            {{- if not (NodeOption .Model "isPage") }}

            if (dao.Count < 1) {
                return null;
            }

            {{- end }}

            return this._mapper.Map<{{ .Model.Name | CamelCase }}>(dao{{ if not (NodeOption .Model "isPage") }}[0]{{ end }});
        }
        {{- else }}
        {{- $pk := .Model.PKProp }}
        {{- if $pk }}

        public {{ .Model.Name | CamelCase }} Get{{ .Model.Name }}({{ $pk.Type }} {{ $pk.Name }})
        {
            {{ $filter | CamelCase }}Dao daoFilter = this._mapper.Map<{{ $filter | CamelCase }}Dao>(filter);
            List<SqlParameter> sqlParams = new List<SqlParameter>()
            {
                new SqlParameter("{{ NodeOptionOr $pk "alt" $pk.Name }}", {{ $pk.Name }}),
            };

            string data = this._connector.GetJson(
              "{{ NodeOption .Model "spGet" }}",
              sqlParams
            );

            {{ if not (NodeOption .Model "isPage") -}}
            List<{{ end }}{{ .Model.Name | CamelCase }}Dao
            {{- if not (NodeOption .Model "isPage") }}>{{ end }} dao = JsonUtils.DeserializeObjectOrDefault(
                data,
                new {{ if not (NodeOption .Model "isPage") }}List<{{- end }}{{ .Model.Name | CamelCase }}Dao{{- if not (NodeOption .Model "isPage") }}>{{ end }}()
            );
            {{- if NodeOption .Model "isPage" }}

            if (dao.Count < 1) {
                return null;
            }

            {{- end }}

            return this._mapper.Map<{{ .Model.Name | CamelCase }}>(dao{{ if not (NodeOption .Model "isPage") }}[0]{{ end }});
        }
        {{- end }}
        {{- end }}
        {{- if not (NodeOption .Model "notPost") }}

        public long Post{{ .Model.Name | CamelCase }}({{ .Model.Name | CamelCase }} model)
        {
            {{ .Model.Name | CamelCase }}Dao payload = this._mapper.Map<{{ .Model.Name | CamelCase }}Dao>(model);
            DatabaseResult result = this._connector.ExecuteWithJsonInput(
                "{{ NodeOption .Model "spPost" }}",
                payload,
            );

            return result.IdResponseCode == (int)DatabaseResult.ResponseCodes.Success
                ? result.EntityId
                : result.IdResponseCode == (int)DatabaseResult.ResponseCodes.DuplicatedName ? -1 : 0;
        }
        {{- end }}
    }
}
