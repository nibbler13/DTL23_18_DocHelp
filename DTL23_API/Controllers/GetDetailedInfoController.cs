using Microsoft.AspNetCore.Mvc;
using DTL23_API.DataClasses;
using DTL23_API.DataHandlers;

namespace DTL23_API.Controllers {
	/// <summary>
	/// Получение детализированных данных по приемам\назначениям
	/// </summary>
	[Route("/[controller]")]
	[ApiController]
	public class GetDetailedInfoController : ControllerBase {
		/// <summary>
		/// Получение детализированных данных по приемам\назначениям в БД
		/// </summary>
		/// <param name="dateBegin">Дата начала периода выборки данных. Формат мм.дд.гггг или гггг-мм-дд</param>
		/// <param name="dateEnd">Дата окончания периода выборки данных. Формат мм.дд.гггг или гггг-мм-дд</param>
		/// <returns>Список, содержащий объекты по приемам\назначениям в формате JSON</returns>
		[HttpGet]
		public async Task<ActionResult<List<DetailedInfo>>> Index(DateTime dateBegin, DateTime dateEnd) {
			List<DetailedInfo> list = null;
			await Task.Run(() => { list = GetListOfDetailedInfo.GetList(dateBegin, dateEnd); });
			return list;
		}
	}
}
