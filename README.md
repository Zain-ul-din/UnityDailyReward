# DailyReward
**Unity Daily Reward Implementation Using System DateTime APIS**


### Features:

- **Redeem Options:** Users can exchange collected rewards for in-game items, currency, upgrades, or trade/sell to other players.

- **User-friendly APIs**: The implementation offers intuitive and straightforward application programming interfaces (APIs) that are designed for easy integration, even for non-technical users.

- **Proven in Production:** Successfully tested and used in a live production environment.

- **Customization:** System is easily customizable to suit specific needs.

- **Easy to Understand:** Everything inside package is self explanatory and easy to understand. 

### Limitations:

- **Local Time/Date:** The system uses local time and date for tracking and managing daily rewards.
### [Used in production](https://play.google.com/store/apps/details?id=com.ga.superhero.skateboard.mini.car.racinggames)

![DailyReward_Demo](https://user-images.githubusercontent.com/78583049/195974892-01a2efec-b015-4309-b32d-7958232b8525.gif)

![Demo_Pic](https://user-images.githubusercontent.com/78583049/195978876-b1b4a5a6-738e-4550-ab4a-210ca1e54562.png)

***

# Usage

```diff
- Version "1.0" documentation may not cover all features of version "2.0".
- But still valuable to checkout. 
```

* Attach ```DailyRewardBtn.cs``` on each button and assign day in inspector.

* Attach ```DailyRewardBtn.cs``` anywhere in scene and assign ```Timer Text```.

* Give Reward on `Btn Click`

***

# References

### **Manager**
![Manager_Ref](https://user-images.githubusercontent.com/78583049/195892469-03ca1823-15f8-4d26-a77f-9e45e9a471ce.jpg)

### **DailyReward Btn**
![daily_rewardbtn_ref](https://user-images.githubusercontent.com/78583049/195967242-9bde2064-2957-4b56-8837-ec5dd332be68.jpg)

### **Reward Button**
![Reward_ref](https://user-images.githubusercontent.com/78583049/195892951-3045e0b4-9b66-4617-b846-c7971413f24f.jpg)

### **2X Reward Button**
![TwoXReward_Ref](https://user-images.githubusercontent.com/78583049/195893083-6c8450b0-d916-44a8-9cae-5e9ebf715f59.jpg)

***

# Docs


### ```DailyRewardManager```
     
**Public Methods:-**

  ```CollectReward ()``` Invokes ```DailyRewardBtn.onRewardCollect``` event of active button.

  ```Collect2XReward ()``` Invokes ```DailyRewardBtn.on2XRewardCollect``` event of active button.
 
**Properties:-** 

  ```AvailableRewardBtn``` return `DailyRewardBtn` if reward will be available.

**Events:-**

  ```OnRewardAvailable``` Invokes when reward available.

**Static Methods:-**

  ```DailyRewardManager Instance``` Readonly

***

### ```DailyRewardBtn```

**Public Methods:-**

```UnityEvent     OnClaimState```

```UnityEvent     OnClaimedState```

```UnityEvent     OnClaimUnAvailableState```

```UnityEvent     onRewardCollect```

```UnityEvent     on2XRewardCollect```

```UnityEvent     onClick```


**Static Methods:-**

```List <DailyRewardBtn> dailyRewardBtns``` static list to access all buttons

***

[Want to Contribute?](https://github.com/Zain-ul-din/DailyReward/blob/master/Scripts/Internal/DailyRewardInternal.cs)

[Report Bug Here!](https://github.com/Zain-ul-din/DailyReward/issues)
