# 개발 1주차

## 폰트 깨짐

|수정 전|수정 후|
|:-----:|:-----:|
|<img src="https://user-images.githubusercontent.com/91234912/139139214-e69c842e-0c22-4260-a9c3-951a1b27e485.PNG">|<img src="https://user-images.githubusercontent.com/91234912/139139227-cdf450b8-d085-41a1-ace6-2b9351bc1e13.PNG">|

한글로 되어 있는 메인 UI 메뉴를 구현하고 싶어서 한글로 메뉴를 작성했으나 유니티가 폰트를 지원하지 않아 폰트를 구웠다. 구운 폰트를 입히니 깨진 글자가 원래대로 돌아온 모습이다.

## 디버깅 테스트

|디버깅 테스트|
|:-----:|
|<img src="https://user-images.githubusercontent.com/91234912/139139219-a0cd239b-da4d-4f28-9d9c-8e63eaa3753f.PNG" height="500" width="2000">|

메인화면을 완성했다.<br>
나중에 메인화면에서 씬 전환이 이루어져야 하기 때문에 enum 열거형을 사용하였고 추후 각 버튼에 맞는 이벤트를 구현 예정이다.<br>
디버깅 테스트가 성공적이고 각 버튼이 본인의 역할에 맞는 이벤트를 발생시키고 있다는 것을 확인했다.<br>
디버그 스크립트 자리에 씬 전환 이벤트를 넣으면 구현이 완료된다.

## 이동 구현

|이동 구현|
|:-----:|
|<img src="https://user-images.githubusercontent.com/91234912/139139223-705d4d51-f768-4b2c-91cc-39332db5e4cd.PNG" height="500" width="2000">|

키보드를 이용한 플레이어의 이동 구현을 완료 했다.<br>
Horizontal, Vertical를 입력받고 Vector3를 이용하기 때문에 W, A, S, D 뿐만 아니라 방향기로도 상하좌우 이동이 가능하다.<br>
Time.deltaTime을 이용하여 컴퓨터 성능이 달라도 동일한 프레임에서 움직이게 하였다.

## 콜라이더 구현

|콜라이더 구현|
|:-----:|
|<img src="https://user-images.githubusercontent.com/91234912/139139226-07d0c9b0-ce0e-46de-8c30-e10ccf512515.PNG" height="500" width="2000">|

캐릭터가 이동하면서 게임 화면 바깥으로 나가는 문제를 수정하였다.<br>
RigidBody 2D의 기능만 이용하여 만들었을 경우 지속해서 벽에 튕기는 현상이 발생하였다.<br>
따라서 Border 테그를 가지고 있는 오브젝트에 부딫히면 플레이어의 해당 방향 벡터가 0이 되게끔 작성하여 튕기지 않게 바꾸었다.<br>
또한 플레이어가 벽에서 떨어지면 해당 방향 Trigger가 flase로 되어 다시 해당 방향으로 이동 할 수 있게 작성하였다.
