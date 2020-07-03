﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Cryptollet.Common.Base;
using Cryptollet.Common.Models;
using Cryptollet.Common.Navigation;
using Cryptollet.Common.Network;
using Cryptollet.Modules.Assets;
using Cryptollet.Modules.Login;
using Cryptollet.Modules.Transactions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Cryptollet.Modules.Wallet
{
    public class WalletViewModel: BaseViewModel
    {
        private INavigationService _navigationService;
        private ICrypoService _crypoService;

        public WalletViewModel(INavigationService navigationService,
                               ICrypoService crypoService)
        {
            _navigationService = navigationService;
            _crypoService = crypoService;
            Assets = new ObservableCollection<Coin>
            {
                new Coin { Name = "Bitcoin", Amount= 0.8934M, Symbol = "BTC", DollarValue = 8452.98M, Change= 5.24M },
                new Coin { Name = "Ethereum", Amount= 8.0175M, Symbol = "ETH", DollarValue = 1825.72M, Change = 1.45M },
                new Coin { Name = "Litecoin", Amount= 24.82M, Symbol = "LTC", DollarValue = 1378.45M, Change = -0.91M },
            };

            LatestTransactions = new ObservableCollection<Transaction>
            {
                new Transaction
                {
                    Status = Constants.TRANSACTION_WITHDRAWN,
                    StatusImageSource = Constants.TRANSACTION_WITHDRAWN_IMAGE,
                    TransactionDate = new DateTime(2019, 8, 19),
                    Amount = 0.021M,
                    DollarValue = 204,
                    Symbol = "BTC"
                },
                new Transaction
                {
                    Status = Constants.TRANSACTION_DEPOSITED,
                    StatusImageSource = Constants.TRANSACTION_DEPOSITED_IMAGE,
                    TransactionDate = new DateTime(2019, 8, 16),
                    Amount = 3.21M,
                    DollarValue = 695.03M,
                    Symbol = "ETH"
                },
                new Transaction
                {
                    Status = Constants.TRANSACTION_DEPOSITED,
                    StatusImageSource = Constants.TRANSACTION_DEPOSITED_IMAGE,
                    TransactionDate = new DateTime(2019, 8, 10),
                    Amount = 37.81M,
                    DollarValue = 250M,
                    Symbol = "NEO"
                },
                new Transaction
                {
                    Status = Constants.TRANSACTION_WITHDRAWN,
                    StatusImageSource = Constants.TRANSACTION_WITHDRAWN_IMAGE,
                    TransactionDate = new DateTime(2019, 8, 5),
                    Amount = 0.021M,
                    DollarValue = 204,
                    Symbol = "BTC"
                },
                new Transaction
                {
                    Status = Constants.TRANSACTION_DEPOSITED,
                    StatusImageSource = Constants.TRANSACTION_DEPOSITED_IMAGE,
                    TransactionDate = new DateTime(2019, 8, 1),
                    Amount = 3.21M,
                    DollarValue = 695.03M,
                    Symbol = "ETH"
                },
            };
        }

        public override async Task InitializeAsync(object parameter)
        {
            var result = await _crypoService.GetLatestPrices();
            Assets[0].DollarValue = Assets[0].Amount * (decimal)result.Bitcoin.Usd;
            Assets[1].DollarValue = Assets[1].Amount * (decimal)result.Ethereum.Usd;
            OnPropertyChanged("Assets");
        }

        private ObservableCollection<Coin> _assets;
        public ObservableCollection<Coin> Assets
        {
            get => _assets;
            set { SetProperty(ref _assets, value); }
        }

        private ObservableCollection<Transaction> _latestTransactions;
        public ObservableCollection<Transaction> LatestTransactions
        {
            get => _latestTransactions;
            set { SetProperty(ref _latestTransactions, value); }
        }

        public ICommand GoToAssetsCommand { get => new Command(async () => await GoToAssets()); }
        public ICommand GoToTransactionsCommand { get => new Command(async () => await GoToTransactions()); }
        public ICommand SignOutCommand { get => new Command(async () => await SignOut()); }

        private async Task SignOut()
        {
            Preferences.Remove(Constants.IS_USER_LOGGED_IN);
            await _navigationService.InsertAsRoot<LoginViewModel>();
        }

        private async Task GoToTransactions()
        {
            await _navigationService.PushAsync<TransactionsViewModel>();
        }

        private async Task GoToAssets()
        {
            await _navigationService.PushAsync<AssetsViewModel>();
        }
    }
}
