using Akavache;
using MvvmHelpers;
using Newtonsoft.Json;
using ShopT.Models;
using ShopT.Models.EnumModels;
using ShopT.Models.LocalModels;
using ShopT.StaticValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ShopT.ViewModels
{
    public class OrderHistoryViewModel : CollectionViewModel
    {
        public ObservableRangeCollection<OrderLocal> Orders { get; } = new ObservableRangeCollection<OrderLocal>();

        //Флаг, означающий что завершенный заказ уже был встречен
        private bool encounteredCompleted = false;

        public OrderHistoryViewModel() 
        {
            GetInitialData = NewAsyncCommand(GetInitial);
            GetMoreData = NewAsyncCommand(GetRemoteData);
        }

        public async Task GetInitial()
        {
            Orders.Clear();
            NextPage = 0;
            encounteredCompleted = false;

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

        //Загружает бренды при смене критерия поиска
        public async Task GetRemoteData()
        {
            if (IsLastPageReached) return;
            try
            {
                HttpClient client = await createUserClient();

                //Отправляем список хэштегов, даже будучи пустым
                HttpResponseMessage response = await client.GetAsync(ApiStrings.HOST +
                                                                        ApiStrings.ORDERS_GET_ORDERS + NextPage);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    List<Order> tempList = JsonConvert.DeserializeObject<List<Order>>(result);

                    if (tempList.Count != 0)
                    {
                        NextPage++;
                    }

                    var localizedList = new List<OrderLocal>();
                    foreach (var item in tempList)
                    {
                        if (item.OrderStatus == OrderStatus.delivered && !encounteredCompleted)
                        {
                            encounteredCompleted = true;
                            localizedList.Add(null);
                        }
                        localizedList.Add(new OrderLocal(item));
                    }
                    Orders.AddRange(localizedList);

                    await BlobCache.LocalMachine.InsertObject(Caches.ORDERS_CACHE.key, (NextPage, Orders.Select(e => e?.Order)), Caches.ORDERS_CACHE.lifeTime);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NextPage = -1; //last page reached
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
            finally 
            {
                IsWorking = false;
            }
        }

        public async Task GetCachedData()
        {
            //Пытаемся вытащить данные из кэша, при неудаче создаем пустую ячейку для предотвращения KeyNotFoundException
            (int, List<Order>) cachedOrders = await new CacheFunctions().tryToGet<(int, List<Order>)>(Caches.ORDERS_CACHE.key, CacheFunctions.BlobCaches.LocalMachine);

            Orders.Clear();

            //В случае если кэш не пуст
            if (cachedOrders != default)
            {
                var localizedList = new List<OrderLocal>();
                foreach (Order brand in cachedOrders.Item2)
                {
                    localizedList.Add(new OrderLocal(brand));
                }
                Orders.AddRange(localizedList);

                NextPage = cachedOrders.Item1;
            }
            //В случае если он пуст
            else
            {
                try
                {
                    await GetInitialData.ExecuteAsync();
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
        }
    }
}
