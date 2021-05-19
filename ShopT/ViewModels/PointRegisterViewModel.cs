using Newtonsoft.Json;
using ShopT.Models;
using ShopT.Models.LocalModels;
using ShopT.StaticValues;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShopT.ViewModels
{
    public class PointRegisterViewModel : CollectionViewModel
    {

        #region properties

        public ObservableCollection<PointRegisterLocal> PointRegisters { get; } = new ObservableCollection<PointRegisterLocal>();

        public PointRegisterViewModel()
        {
            GetInitialData = NewAsyncCommand(GetInitial);
            GetMoreData = NewAsyncCommand(GetRemoteData);
        }

        #endregion

        #region methods

        public async Task GetInitial()
        {
            PointRegisters.Clear();
            NextPage = 0;

            try
            {
                await GetMoreData.ExecuteAsync();
            }
            catch (NoConnectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw CheckIfConnectionException(e);
            }
        }

        public async Task GetRemoteData() 
        {
            PointRegisters.Clear();

            try
            {
                HttpClient client = await createUserClient();

                //Получение всех продуктов по id меню
                HttpResponseMessage response = await client.GetAsync(ApiStrings.HOST +
                                                                        ApiStrings.POINT_REGISTERS_CONTROLLER + NextPage);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    List<PointRegister> tempList = JsonConvert.DeserializeObject<List<PointRegister>>(result);
                    foreach (var item in tempList)
                    {
                        PointRegisters.Add(new PointRegisterLocal() { PointRegister = item });
                    }
                }
            }
            catch (NoConnectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw CheckIfConnectionException(e);
            }
        }

        #endregion

    }
}
