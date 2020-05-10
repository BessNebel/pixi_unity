import UISprite from './UI/UISprite';
import UIPopup from './UI/UIPopup';
import UILabel from './UI/UILabel';
import UIButton from './UI/UIButton';

export  class UISpriteImageBackgroundMessageUISprite extends UISprite {
	public ImageBackgroundMessage: UISpriteTextMessageUILabelTextTestUILabel;

	constructor(textureName: string, x: number, y: number, ImageBackgroundMessage: UISpriteTextMessageUILabelTextTestUILabel) {
		super(textureName, x, y);

		this.ImageBackgroundMessage = ImageBackgroundMessage;
		this.addChild(this.ImageBackgroundMessage);
	};
}

export  class UISpriteTextMessageUILabelTextTestUILabel extends UISprite {
	public TextMessage: UILabel;
	public TextTest: UILabel;

	constructor(textureName: string, x: number, y: number, TextMessage: UILabel, TextTest: UILabel) {
		super(textureName, x, y);

		this.TextMessage = TextMessage;
		this.addChild(this.TextMessage);

		this.TextTest = TextTest;
		this.addChild(this.TextTest);
	};
}

export  class UISpriteTextLabelUILabel extends UISprite {
	public TextLabel: UILabel;

	constructor(textureName: string, x: number, y: number, TextLabel: UILabel) {
		super(textureName, x, y);

		this.TextLabel = TextLabel;
		this.addChild(this.TextLabel);
	};
}

export  class UISpriteButtonCancelUIButtonButtonOkUIButton extends UISprite {
	public ButtonCancel: UIButtonTextLabelUILabel;
	public ButtonOk: UIButtonTextLabelUILabel;

	constructor(textureName: string, x: number, y: number, ButtonCancel: UIButtonTextLabelUILabel, ButtonOk: UIButtonTextLabelUILabel) {
		super(textureName, x, y);

		this.ButtonCancel = ButtonCancel;
		this.addChild(this.ButtonCancel);

		this.ButtonOk = ButtonOk;
		this.addChild(this.ButtonOk);
	};
}

export  class UIButtonTextLabelUILabel extends UIButton {
	public TextLabel: UILabel;

	constructor(background: string, x: number, y: number, TextLabel: UILabel) {
		super(background, x, y);

		this.TextLabel = TextLabel;
		this.addChild(this.TextLabel);
	};
}
