using Microsoft.AspNetCore.Mvc;
using DTL23_API.DataClasses;
using DTL23_API.DataHandlers;

namespace DTL23_API.Controllers {
	/// <summary>
	/// Получение данных по сопоставлению услуг
	/// </summary>
	[Route("/[controller]")]
	[ApiController]
	public class GetServiceComparisonController : Controller {
		/// <summary>
		/// Получение данных по сопоставлению услуг из БД
		/// </summary>
		/// <returns>Список, содержащий объекты по сопоставлению услуг в формате JSON</returns>
		[HttpGet]
		public async Task<ActionResult<List<DataSetServiceComparison>>> Index() {
			List<DataSetServiceComparison> list = null;
			await Task.Run(() => { list = GetListOfDataSetServiceComparison.GetList(); });
			return list;
		}
	}
}
