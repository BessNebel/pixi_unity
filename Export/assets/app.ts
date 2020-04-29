
    import * as PIXI from 'pixi.js';
    import UIButton from './UIButton';
    import UISprite from './UISprite';
    import UILabel from './UILabel';
const images = [
"images/PopupBackground.png",
"images/PopupButton.png",
];

    const app = new PIXI.Application({ width: 760, height: 540, transparent: false });
    app.renderer.backgroundColor = 0xCCCCCC;
    document.body.appendChild(app.view);

    PIXI
      .loader
      .add(Object.values(images))
      .load(setup);
function setup() {

      class Popup extends UISprite {
    		public Button?: UIButton;

    		addButton(button: UIButton) {
    			this.Button = button;
    			this.addChild(button);
  			}
    	};
let WrapperPopup = new Popup(app.stage);
WrapperPopup.move(190, 135);

let ImageBackground = new UISprite(WrapperPopup, "images/PopupBackground.png", 0, 0);

let TextMessage = new UILabel(WrapperPopup, "TextMessage", 45, 0x009C13, 190, 132);
WrapperPopup.addLabel(TextMessage);

let ButtonOk = new UIButton(WrapperPopup, "images/PopupButton.png", 87, 180);
WrapperPopup.addButton(ButtonOk)

let TextButtonLabel = new UILabel(ButtonOk, "Ok", 30, 0xFFFFFF, 103, 35);
ButtonOk.addLabel(TextButtonLabel);

};
