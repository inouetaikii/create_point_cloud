# 3次元点群データ作成プログラム

深層学習等で使える3次元点群データセットを自前で作成するために作りました。

# デモ動画

https://youtu.be/vL-MbB5EAuA

YouTubeに投稿したので気になる方はチェックしてください。

# 私の実行環境

Unity(2020.3.4f1), Python(3.9), Open3D(0.18.0)

# 実行方法

## 1, Unityでシミュレーション環境を作成

![Unityの画面](https://github.com/inouetaikii/create_point_cloud/assets/168691211/2cb68f6a-c9aa-4a2f-a4ea-16cebc7fbdb2)

点群を取得したい視点のカメラ(画像ではcamera)に4つのファイルをアタッチする。FOVの値はOpen3Dでカラー画像・深度画像から点群を作成する際に用いるため記憶しておく。

## 2, o3d_create_point_cloud.pyを実行

![color_and_depth_images](https://github.com/inouetaikii/create_point_cloud/assets/168691211/d66adf1f-c003-423e-b601-374382d30243)

カラー画像・深度画像を表示

![point_cloud_data](https://github.com/inouetaikii/create_point_cloud/assets/168691211/0289b4cf-4ee1-4529-a362-b1f0bff5f911)

作成された点群

![point_cloud_data_valuation](https://github.com/inouetaikii/create_point_cloud/assets/168691211/285b14c7-c0c5-43ed-98d5-2443a04090f0)

Unityで取得した対象物の位置・姿勢をもとにモデル点群を移動させ、作成された点群と一致するか確認

# 注意点

私の対象物はスタンフォード大学が提供するバニー9体だったので、時間経過によって1体ずつ落下させる「Iteration.cs」というファイルを作成しました。
不必要な方はカメラにアタッチしなくてもよいです。
また「SavePosQuaToText.cs」では、対象物の名前のリストが必要です。私の場合、9体のバニーでしたが、必要に応じて変更してください。

