# JapaneseDateClassCSharp
文字、数字、日付を和暦変換します。

### Sample
```cs
/// <summary>
/// 日付を和暦に変換します。
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void dateTimePicker_ValueChanged(object sender, EventArgs e)
{
    JapaneseDate jpd = new JapaneseDate(dateTimePicker.Value);
    lblDateRetDate.Text = jpd.Date;
}
```


## 環境・言語
* Visual Studio 2015
* C#

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


## 日付として変換できなかった時の挙動

## テスト環境
直ぐに動作確認お行えるようFormを用意いています。

![デバッグ1](https://github.com/s-tsurumaki/JapaneseDateClass_CSharp/blob/master/img/debug1.png)
