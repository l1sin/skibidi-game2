mergeInto(LibraryManager.library, {

  GetLanguage: function () {
    var lang = ysdk.environment.i18n.lang;

    var bufferSize = lengthBytesUTF8(lang) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(lang, buffer, bufferSize);
    return buffer;
  },

  Rate: function () 
  {
    ysdk.feedback.requestReview()
    .then(({ feedbackSent }) => {
      console.log(feedbackSent);
    })
  },

  WatchAdAdd: function () {
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
          console.log('Video ad open.');
        },
        onRewarded: () => {
          myGameInstance.SendMessage('MainMenuController', 'GetAdReward');
          console.log('Rewarded 500 money');
        },
        onClose: () => {
          console.log('Video ad closed.');
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
      }
    })
  },

  WatchAdDouble: function () {
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
          console.log('Video ad open.');
        },
        onRewarded: () => {
          myGameInstance.SendMessage('Level Finished', 'DoubleReward');
          console.log('Rewarded double money');
        },
        onClose: () => {
          console.log('Video ad closed.');
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
      }
    })
  },

  SaveExtern: function (data) {
    if (player.getMode() === 'lite')
    {
      console.log("Saving. No auth. No cloud save, only local.");
    }
    else
    {
      var dataString = UTF8ToString(data);
      var myobj = JSON.parse(dataString);
      player.setData(myobj, true);
      console.log("Saving. Auth. Save to cloud.");
    }
  },

  LoadExtern: function () {
    if (player.getMode() === 'lite')
    {
      console.log("Loading. No auth. Use local save.");
      myGameInstance.SendMessage('SaveManager', 'LoadDataLocal');
    }
    else
    {
      player.getData().then(_data => {
        console.log("Loading. Auth. Use cloud save.");
        const myJSON = JSON.stringify(_data);
        myGameInstance.SendMessage('SaveManager', 'LoadDataCloud', myJSON);
      });
    }
  },

  FullScreenAd: function () {
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onClose: function(wasShown) {
          // some action after close
        },
        onError: function(error) {
          // some action on error
        }
      }
    })
  },

  CallPurchaseMenu: function (pID, name) {
    var pIDstring = UTF8ToString(pID);
    var namestring = UTF8ToString(name);
    payments.purchase({ id: pIDstring }).then(purchase => {
      myGameInstance.SendMessage(namestring, 'BuyAllOnClick', purchase.purchaseToken); 
    }).catch(err => {
        // Покупка не удалась: в консоли разработчика не добавлен товар с таким id,
        // пользователь не авторизовался, передумал и закрыл окно оплаты,
        // истекло отведенное на покупку время, не хватило денег и т. д.
    })
  },

  GetPrice: function (id) {
    var item = gameShop[id]
    var price = item.priceValue;
    var bufferSize = lengthBytesUTF8(price) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(price, buffer, bufferSize);
    return buffer;
  },

  GetYanIcon: function () {
    var url = gameShop[0].getPriceCurrencyImage('medium')
    myGameInstance.SendMessage('MainMenuController', 'SetYanTexture', url); 
  },

  GameReady: function () {
    ready();
  },

  ConsumePurchase: function (purchaseToken) {
    var tokenString = UTF8ToString(purchaseToken);
    payments.consumePurchase(tokenString);
    console.log('Purchase consumed');
  },

  CheckPurchases: function () {
    payments.getPurchases().then(purchases => purchases.forEach((purchase) =>{
      var info = purchase.productID + ',' + purchase.purchaseToken;
      console.log(info);
      myGameInstance.SendMessage('MainMenuController', 'CheckPurchase', info); 
    }));
  },

  CallRate: function()
  {
    ysdk.feedback.canReview()
    .then(({ value, reason }) => {
      if (value) 
      {
        myGameInstance.SendMessage('MainMenuController', 'howRateWindow');
      } 
      else 
      {
        console.log(reason)
      }
    })
  }
});