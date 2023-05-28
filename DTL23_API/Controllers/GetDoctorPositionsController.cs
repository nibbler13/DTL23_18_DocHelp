using DTL23_API.DataHandlers;
using Microsoft.AspNetCore.Mvc;

namespace DTL23_API.Controllers {
	/// <summary>
	/// Получение справочника по должностям
	/// </summary>
	[Route("/[controller]")]
	[ApiController]
	public class GetDoctorPositionsController : ControllerBase {
		/// <summary>
		/// Получение справочника по должностям из БД
		/// </summary>
		/// <returns>Список должностей</returns>
		[HttpGet]
		public async Task<ActionResult<List<string>>> Index() {
			List<string> positions = null;
			await Task.Run(() => { positions = GetListOfDoctorPositions.GetList(); });
			return positions;
		}
	}
}
