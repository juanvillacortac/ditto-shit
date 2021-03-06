{{- $filter := NodeOption .Model "filter" -}}
{{- $pageItem := (NodeOptionOr .Model "pageItem" .Model.Name) | CamelCase -}}
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { {{ .Model.Name }} } from '../../models/{{ NodeOption .Root "namespace" | KebabCase }}/{{ .Model.Name | KebabCase }}'
{{- if $filter }}
import { {{ $filter }} } from "../../models/{{ NodeOption .Root "namespace" | KebabCase }}/{{ $filter | KebabCase }}";
{{- end }}
{{- if ne $pageItem .Model.Name }}{{with Model $pageItem}}
import { {{ $pageItem }} } from "../../models/{{ NodeOption $.Root "namespace" | KebabCase }}/{{ $pageItem | KebabCase }}";
{{- end }}{{ end }}

@Injectable({
  providedIn: "root",
})
export class {{ .Model.Name }}Service {
  constructor(
    private _httpClient: HttpClient,
    private _httpHelpersService: HttpHelpersService
  ) {}
  {{- if and $filter (not (NodeOption .Model "singular")) }}

  get{{ .Model.Name | Plural }}List(filter = new {{ $filter }}()) {
    return this._httpClient.get<{{ .Model.Name }}[]>(
      '/{{ .Model.Name | Plural | KebabCase }}',
      {
        params: this._httpHelpersService.getHttpParamsFromPlainObject(
          filter,
          false
        ),
      }
    );
  }
  {{- end }}
  {{- if NodeOption .Model "singularFromFilter" }}

  get{{ .Model.Name }}(filter = new {{ $filter }}()) {
    return this._httpClient.get<{{ .Model.Name }}>('/{{ .Model.Name | Plural | KebabCase }}', {
      params: this._httpHelpersService.getHttpParamsFromPlainObject(
        filter,
        false
      ),
    });
  }
  {{- else }}
  {{- $pk := .Model.PKProp }}
  {{- if $pk }}

  get{{ .Model.Name }}({{ $pk.Name }}: {{ $pk.Type }}) {
    return this._httpClient.get<{{ .Model.Name }}>(`/{{ .Model.Name | Plural | KebabCase }}/${ {{- $pk.Name -}} }`);
  }
  {{- end }}
  {{- end }}
  {{- if not (NodeOption .Model "notPost") }}

  post{{ .Model.Name }}(model: {{ $pageItem }}) {
    return this._httpClient.post<number>('/{{ .Model.Name | Plural | KebabCase }}', model);
  }
  {{- end }}
}