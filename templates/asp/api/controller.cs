{{ $namespace := NodeOption .Root "cs_namespace" -}}
{{- $filter := NodeOption .Model "filter" -}}
using System.Collections.Generic;
using System.Linq;
using System.Net;

using AutoMapper;

using {{ $namespace }}.Domain.{{ .Model.Name | CamelCase }}Domain;
{{- range $key, $dep := ModelDeps .Model }}
// using {{ $namespace }}.Domain.{{ $dep.Name | CamelCase }}Domain;
{{- end }}

using {{ $namespace }}.API.ViewModels.{{ .Model.Name | CamelCase }};
{{- range $key, $dep := ModelDeps .Model }}
// using {{ $namespace }}.API.ViewModels.{{ $dep.Name | CamelCase }};
{{- end }}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace {{ $namespace }}.API.Controllers
{
    [Route("{{ .Model.Name | Plural | KebabCase }}")]
    [ApiController]
    public class {{ .Model.Name | CamelCase }}Controller : ControllerBase
    {

        private readonly ILogger<{{ .Model.Name | CamelCase }}Controller> _logger;
        private readonly I{{ .Model.Name | CamelCase }}Service _service;
        private readonly IMapper _mapper;

        public {{ .Model.Name | CamelCase }}Controller(ILogger<{{ .Model.Name | CamelCase }}Controller> logger, I{{ .Model.Name | CamelCase }}Service service, IMapper mapper)
        {
            this._logger = logger;
            this._service = service;
            this._mapper = mapper;
        }
        {{- if and $filter (not (NodeOption .Model "singular")) }}

        [HttpGet]
        [ProducesResponseType(typeof(List<{{ .Model.Name | CamelCase }}ViewModel>), (int)HttpStatusCode.OK)]
        public ActionResult<List<{{ .Model.Name | CamelCase }}ViewModel>> GetList([FromQuery] {{ $filter | CamelCase }}ViewModel filter)
        {
            List<{{ .Model.Name | CamelCase }}> result = this._service.Get{{ .Model.Name | Plural | CamelCase }}List(this._mapper.Map<{{ $filter | CamelCase }}>(filter));
            return new JsonResult(this._mapper.Map<List<{{ .Model.Name | CamelCase }}ViewModel>>(result));
        }
        {{- end }}
        {{- if NodeOption .Model "singularFromFilter" }}

        [HttpGet]
        [ProducesResponseType(typeof({{ .Model.Name | CamelCase }}ViewModel), (int)HttpStatusCode.OK)]
        public ActionResult<{{ .Model.Name | CamelCase }}ViewModel> Get([FromQuery] {{ $filter | CamelCase }}ViewModel filter)
        {
            {{ .Model.Name | CamelCase }} result = this._service.Get{{ .Model.Name | CamelCase }}(this._mapper.Map<{{ $filter | CamelCase }}>(filter));
            return new JsonResult(this._mapper.Map<{{ .Model.Name | CamelCase }}ViewModel>(result));
        }
        {{- else }}
        {{- $pk := .Model.PKProp }}
        {{- if $pk }}

        [HttpGet("{ {{- $pk.Name -}} }")]
        [AllowAnonymous]
        [ProducesResponseType(typeof({{ .Model.Name | CamelCase }}ViewModel), (int)HttpStatusCode.OK)]
        public ActionResult<{{ .Model.Name | CamelCase }}ViewModel> Get({{ $pk.Type }} {{ $pk.Name }})
        {
            {{ .Model.Name | CamelCase }} result = this._service.Get{{ .Model.Name | Plural }}(this._mapper.Map<{{ $filter | CamelCase }}>({{ $pk.Name }}));
            return new JsonResult(this._mapper.Map<{{ .Model.Name | CamelCase }}ViewModel>(result));
        }
        {{- end }}
        {{- end }}

        {{- if not (NodeOption .Model "notPost") }}
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public ActionResult Post([FromBody] {{ .Model.Name | CamelCase }}ViewModel viewModel)
        {
            long result = this._service.Post{{ .Model.Name | CamelCase }}(this._mapper.Map<{{ .Model.Name | CamelCase }}>(viewModel));
            return new JsonResult(result);
        }
        {{- end }}
    }
}
