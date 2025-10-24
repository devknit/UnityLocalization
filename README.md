# Localization

言語に応じた文字列を管理し、アクセスできる仕組みを提供しているパッケージです。

## 管理できる言語

プロジェクトで使用する言語毎にプリプロセッサを定義することで、対象の言語に対応した文字列を管理できるようになります。

| 対応言語   | プリプロセッサ定義               |
| ------ | ----------------------- |
| 日本語    | デフォルトで管理可能              |
| 英語     | デフォルトで管理可能              |
| ドイツ語   | LOCALE_LANGUAGE_DE      |
| スペイン語  | LOCALE_LANGUAGE_ES      |
| フランス語  | LOCALE_LANGUAGE_FR      |
| イタリア語  | LOCALE_LANGUAGE_IT      |
| 韓国語    | LOCALE_LANGUAGE_KO      |
| 中国語簡体字 | LOCALE_LANGUAGE_ZH_HANS |
| 中国語繁体字 | LOCALE_LANGUAGE_ZH_HANT |

## デフォルト言語

ランタイムで選択されるデフォルトの言語は OS で選択されている言語が自動で選ばれる状態が基本ですが、以下の内１つだけプリプロセッサを定義することで言語を固定できます

| 選択言語   | プリプロセッサ定義               |
| ------ | ----------------------- |
| 日本語    | LOCALE_LANGUAGE_JA_DEFAULT              |
| 英語     | LOCALE_LANGUAGE_EN_DEFAULT              |
| ドイツ語   | LOCALE_LANGUAGE_DE_DEFAULT      |
| スペイン語  | LOCALE_LANGUAGE_ES_DEFAULT      |
| フランス語  | LOCALE_LANGUAGE_FR_DEFAULT      |
| イタリア語  | LOCALE_LANGUAGE_IT_DEFAULT      |
| 韓国語    | LOCALE_LANGUAGE_KO_DEFAULT      |
| 中国語簡体字 | LOCALE_LANGUAGE_ZH_HANS_DEFAULT |
| 中国語繁体字 | LOCALE_LANGUAGE_ZH_HANT_DEFAULT |

※定義が重複した場合は上にある定義が優先されます

## 言語の変更

以下の API によって取得できる言語を変更することが出来ます。

- Localize.SetLocale( Locale)
- Localize.SetLocaleCode( string)

プリプロセッサで `LOCALE_LANGUAGE_FIXED` を定義することで変更を無効化することが出来ます。

`LOCALE_LANGUAGE_FIXED` を定義した場合は [デフォルト言語](#デフォルト言語) のプリプロセッサを１つ定義する必要があります


# Localization Table

Unity の メニューにある `Tools/Localization` を選択することでローカライズテーブルのエディタウィンドウが表示されます。

# 使用方法

## LocalizeListener コンポーネント

LocalizeListener コンポーネントを追加し、Reference で用意したテーブル、エントリーを指定します。

Update String を設定することで Runtime で文字列がコールバックされます。

以下のコンポーネントが付与されているオブジェクトに LocalizeListener を付与した場合自動的に Update String が設定されます

- TextMesh Pro - Text
- TextMesh Pro - Text (UI)
- Text
- TextMesh

コールバックはオブジェクトがアクティブになった時と、言語設定を変更した場合に呼び出されます。

## APIの呼び出しによる取得

以下の API にテーブル名とエントリー名を渡すことで文字列が取得できます。

- Localize.GetString
- Localize.TryGetString

LocalizeListener のコールバックとは異なり、言語設定を変えた場合には任意で再取得する必要があります
