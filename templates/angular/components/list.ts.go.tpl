import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { ColumnD } from "src/app/models/common/columnsd";
import { BreadcrumbService } from "src/app/design/breadcrumb.service";
import { {{ .Model.Name }} } from "../../../models/{{ NodeOption .Root "namespace" | KebabCase }}/{{ .Model.Name | KebabCase }}";

@Component({
  selector: "app-{{ .Model.Name | KebabCase }}-list",
  templateUrl: "./{{ .Model.Name | KebabCase }}-list.component.html",
  styleUrls: ["./{{ .Model.Name | KebabCase }}-list.component.scss"],
})
export class {{ .Model.Name | CamelCase }}ListComponent implements OnInit {
  table: any[] = [];
  {{ .Model.Name | LowerCamelCase | Plural }}: {{ .Model.Name | CamelCase }}[];

  showFilters = false;

  constructor(
    public breadcrumbService: BreadcrumbService,
    private router: Router
  ) {
    this.breadcrumbService.setItems([
      { label: "Cambiame" },
      { label: "Cambiame"  },
      { label: "{{NodeOption .Model "displayPlural"}}", routerLink: ["/changeme/changeme/{{.Model.Name | KebabCase}}-list"] }
    ]);
  }

  cols: ColumnD<{{ .Model.Name }}>[] = [
    {{- range .Model.Props}}{{ if NodeOption . "display" }}
    {
      {{- if eq .Type "Date" }}
      template: (obj) =>
        new Date(obj.{{ .Name }}).getFullYear() !== 1900
          ? toDate(obj.{{ .Name }})
          : null,
      {{- else }}
      template: (obj) => obj.{{ .Name }},
      {{- end }}
      field: "{{ .Name }}",
      header: "{{ NodeOption . "display" }}",
      display: "table-cell",
    },
    {{- end }}{{- end }}
  ];

  ngOnInit(): void {}
}

{{- range .Model.Props}}{{ if and (NodeOption . "display") (eq .Type "Date") }}
const toDate = (str: string | Date) => {
  const d = new Date(str);
  const padLeft = (n: number) => ("00" + n).slice(-2);
  const dformat = [
    padLeft(d.getDate()),
    padLeft(d.getMonth() + 1),
    d.getFullYear(),
  ].join("/");
  return dformat;
};
{{- end }}{{- end }}