{{- $filter := NodeOption .Model "filter" -}}
<div class="card">
  <div class="p-d-flex p-ai-center p-jc-between p-flex-column p-flex-md-row">
    <div class="p-text-left p-text-bold p-sm-12 p-md-6 p-lg-3 p-col-12">
      <i class="p-text-bold pi pi-book"></i> {{ NodeOptionOr .Model "displayPlural" .Model.Name }}
    </div>

    <div>
      <button
        pButton
        pRipple
        icon="pi pi-plus"
        class="p-button-success"
        [ngClass]="{ 'p-button-danger': showDialog }"
        (click)="new()"
        pTooltip="Crear nuevo"
        tooltipPosition="top"
      ></button>
      {{- if $filter }}
      <button
        pButton
        pRipple
        icon="pi pi-filter"
        class="p-button-help p-ml-2"
        [ngClass]="{ 'p-button-danger': showFilters }"
        (click)="showFilters = !showFilters"
        pTooltip="Filtros"
        tooltipPosition="top"
      ></button>
      {{- end }}
    </div>
  </div>
</div>

<div class="card p-m-0">
  <p-table
    [resizableColumns]="false"
    styleClass="p-datatable-responsive-demo p-datatable p-component p-datatable-hoverable-rows"
    #dt
    [columns]="cols"
    [rowHover]="true"
    [value]="{{ .Model.Name | LowerCamelCase | Plural }}"
    [paginator]="true"
    [rows]="10"
    [showCurrentPageReport]="true"
    currentPageReportTemplate="Mostrando {first} hasta {last} de {totalRecords} registros"
    [rowsPerPageOptions]="[10, 25, 50]"
  >
    <ng-template pTemplate="header" let-columns>
      <tr>
        <ng-container *ngFor="let col of columns">
          <th
            [ngStyle]="{ display: col.display, 'overflow-wrap': 'break-word' }"
            pResizableColumn
            [pSortableColumn]="col.field"
          >
            {{ "{{ col.header }}" }}
            <p-sortIcon [field]="col.field"></p-sortIcon>
          </th>
        </ng-container>
        <th></th>
      </tr>
    </ng-template>

    <ng-template pTemplate="body" let-{{ .Model.Name | LowerCamelCase }} let-columns="columns">
      <tr>
        <ng-container *ngFor="let col of columns">
          <td
            [ngStyle]="{ display: col.display, 'overflow-wrap': 'break-word' }"
            class="ui-resizable-column"
          >
            {{- range .Model.Props }}{{ if NodeOption . "active" }}
            <app-active-label
              *ngIf="col.field === '{{ .Name }}'; else textCell"
              [active]="{{ $.Model.Name | LowerCamelCase }}.{{ .Name }}"
            >
            </app-active-label>
            {{- end }}{{ end }}
            <ng-template #textCell>
              <span>{{"{{"}} col.template({{ .Model.Name | LowerCamelCase }}) {{"}}"}}</span>
            </ng-template>
          </td>
        </ng-container>
        <td>
          <button
            pButton
            pRipple
            icon="pi pi-pencil"
            class="p-button-rounded p-button-success p-mr-2"
            (click)="edit({{ .Model.Name | LowerCamelCase }})"
            pTooltip="Editar"
          ></button>
        </td>
      </tr>
    </ng-template>

    <ng-template pTemplate="footer" let-columns>
      <ng-container>
        <tr *ngIf="{{ .Model.Name | LowerCamelCase | Plural }}.length == 0">
          <td
            [ngStyle]="{ 'text-align': 'center' }"
            [attr.colspan]="columns.length + 1"
          >
            <p-message
              severity="info"
              text="No existen registros."
              styleClass="p-col-12 p-mr-2"
            ></p-message>
          </td>
        </tr>
      </ng-container>
    </ng-template>
    <ng-template pTemplate="paginatorleft"> </ng-template>
    <ng-template pTemplate="paginatorright"> </ng-template>
  </p-table>
</div>