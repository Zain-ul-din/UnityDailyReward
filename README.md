# DailyReward
**Unity Daily Reward Implementation Using System DateTime APIS**

### [Used in production](https://play.google.com/store/apps/details?id=com.ga.superhero.skateboard.mini.car.racinggames)

![DailyReward_Demo](https://user-images.githubusercontent.com/78583049/195974892-01a2efec-b015-4309-b32d-7958232b8525.gif)

![Demo_Pic](https://user-images.githubusercontent.com/78583049/195978876-b1b4a5a6-738e-4550-ab4a-210ca1e54562.png)

***

# Usage

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

```List <DailyRewardBtn> dailyRewardBtns``` static list to acess all buttons

***

[Want to Contribute?](https://github.com/Zain-ul-din/DailyReward/blob/master/Scripts/Internal/DailyRewardInternal.cs)

[Report Bug Here!](https://github.com/Zain-ul-din/DailyReward/issues)
