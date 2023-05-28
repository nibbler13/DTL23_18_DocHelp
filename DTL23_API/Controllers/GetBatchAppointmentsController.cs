using Microsoft.AspNetCore.Mvc;
using DTL23_API.DataClasses;
using DTL23_API.DataHandlers;
using DTL23_API.Helpers;

namespace DTL23_API.Controllers {
	/// <summary>
	/// Получение данных по пакетным назначениям
	/// </summary>
	[Route("/[controller]")]
	[ApiController]
	public class GetBatchAppointmentsController : ControllerBase {
		/// <summary>
		/// Получение данных по пакетным назначениям из БД
		/// </summary>
		/// <param name="mkbCode">МКБ-10 код (опционально)</param>
		/// <returns>Список, содержащий объекты с пакетными назначениями</returns>
		/// <exception cref="ArgumentException">mkbCode должен соответствовать одному 
		/// из кодов справочника МКБ-10</exception>
		[HttpGet]
		public async Task<ActionResult<List<BatchAppointment>>> Index(string? mkbCode) {
			if (!string.IsNullOrEmpty(mkbCode) && !MKB10.IsMkb10StringCorrect(mkbCode))
				throw new ArgumentException("МКБ-10 код не соответствует формату");

			List<BatchAppointment> list = null;
			await Task.Run(() => { list = GetListOfBatchAppointments.GetList(mkbCode); });
			return list;
		}
	}
}
