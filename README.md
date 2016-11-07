# JapaneseDateClassCSharp
文字、数字、日付を和暦変換します。

# 環境・言語
* Visual Studio 2015
* C#

# Sample
コードでのサンプルです。

```cs
JapaneseDate jpd = new JapaneseDate(DateTime.Now);
Console.WriteLine(jpd.NendoDate); // 平成28年1月1日

jpd = new JapaneseDate("H280101");
Console.WriteLine(jpd.NendoDate); // 平成28年1月1日

jpd = new JapaneseDate(4280101);
Console.WriteLine(jpd.NendoDate); // 平成28年1月1日

```

# Custom Controls
カスタムコントロールは以下のものが利用可能です。

| Name  | Base |Remarks|
| :--- | :---: |------------- |
| JapaneseDateTextBox  | TextBox  ||

# Custom Controls Demo
カスタムコントロールのデモです

### JapaneseDateTextBox

![JapaneseDateTextBoxDemo](https://github.com/s-tsurumaki/JapaneseDateClass_CSharp/blob/master/img/JapaneseDateTextBox.gif)

### JapaneseDateTextBox(yyyy/mm/dd)
DateTime.TryParseで成功するものは変換可能です。

![JPNEraTextBoxEdityyyymmdd](https://github.com/s-tsurumaki/JapaneseDateClass_CSharp/blob/master/img/JPNEraTextBoxEdityyyymmdd.gif)

### JapaneseDateTextBox(7int)
![JPNEraTextBoxEdit7int](https://github.com/s-tsurumaki/JapaneseDateClass_CSharp/blob/master/img/JPNEraTextBoxEdit7int.gif)

### JapaneseDateTextBox(7int)
![JPNEraTextBoxEdit7int](https://github.com/s-tsurumaki/JapaneseDateClass_CSharp/blob/master/img/JPNEraTextBoxEdit7int.gif)

### JPNEraTextBoxEditAlphabet
![JPNEraTextBoxEditAlphabet](https://github.com/s-tsurumaki/JapaneseDateClass_CSharp/blob/master/img/JPNEraTextBoxEditAlphabet.gif)

## テスト環境
直ぐに動作確認お行えるようFormを用意いています。

![デバッグ1](https://github.com/s-tsurumaki/JapaneseDateClass_CSharp/blob/master/img/debug1.png)

## 変換可能な値
### DateTime
| DateTime  | Conversion |Remarks|
| :--- | :---: |------------- |
| 2016/01/01  | 平成28年1月1日  |日付として変換可能なもの|
| 2016.01.01  | 平成28年1月1日  |日付として変換可能なもの|
| 2016/1/1  | 平成28年1月1日  |日付として変換可能なもの|
| 2016.1.1  | 平成28年1月1日  |日付として変換可能なもの|
### String
| String  | Conversion |Remarks|
| :--- | :---: |------------- |
| 2016/01/01  | 平成28年1月1日  |日付として変換可能なもの|
| 2016.01.01  | 平成28年1月1日  |日付として変換可能なもの|
| 2016/1/1  | 平成28年1月1日  |日付として変換可能なもの|
| 2016.1.1  | 平成28年1月1日  |日付として変換可能なもの|
| H28/01/01  | 平成28年1月1日  |元号をアルファベット表記したもの|
| H28.01.01  | 平成28年1月1日  |元号をアルファベット表記したもの|
| H28/1/1  | 平成28年1月1日  |元号をアルファベット表記したもの
| H28.1.1  | 平成28年1月1日  |元号をアルファベット表記したもの|
### int
| int  | Conversion |Remarks|
| :--- | :---: |------------- |
| 4280101  | 平成28年1月1日  |元号を1～4で表記したもの|

## 元号記号について
| Symbol  | Japanese era |Remarks|
| :---: | :---: |------------- |
| 1  | 明治  ||
| 2  | 大正  ||
| 3  | 昭和  ||
| 4  | 平成  ||
| M  | 明治  ||
| T  | 大正  ||
| S  | 昭和  ||
| H  | 平成  ||

## 年度について
4月1日から翌年の3月31日までを現年度とする概念も取得可能です。

年度を取得する場合はNendoDateメソッドを利用します。

### Sample
```cs
JapaneseDate jpd = new JapaneseDate(DateTime.Now);
Console.WriteLine(jpd.NendoDate);
```

### 年度変換の例
| 日付  | Conversion |Remarks|
| :--- | :---: |------------- |
| 2016/03/31  | 平成27年3月31日  ||
| 2016/04/01  | 平成28年4月1日  ||
| 2017/01/01  | 平成28年1月1日  ||
| 2017/03/31  | 平成28年3月31日  ||


## 日付として変換できなかった時の挙動


