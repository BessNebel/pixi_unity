import UISprite from './UI/UISprite';
import UIPopup from './UI/UIPopup';
import UILabel from './UI/UILabel';
import UIButton from './UI/UIButton';

export default class PopupMessage extends UIPopup {
	public ImageBackground: UISpriteImageBackgroundMessageUISprite;
	public ImageHeader: UISpriteTextLabelUILabel;
	public ImageFooter: UISpriteButtonCancelUIButtonButtonOkUIButton;

	constructor(x: number, y: number) {
		super(x, y);

		const TextMessage = new UILabel("Message", 35, 0x009C13, 120, 40, );
		const TextTest = new UILabel("Text", 35, 0xFF7200, 480, 110, );
		const ImageBackgroundMessage = new UISpriteTextMessageUILabelTextTestUILabel("images/PopupContentBackground.png", 15, 127.5, TextMessage, TextTest);
		const ImageBackground = new UISpriteImageBackgroundMessageUISprite("images/PopupBackground.png", 0, 0, ImageBackgroundMessage);

		this.ImageBackground = ImageBackground;
		this.addChild(this.ImageBackground);

		const TextLabel = new UILabel("Label", 50, 0xFFE700, 222, 46.50001, );
		const ImageHeader = new UISpriteTextLabelUILabel("images/PopupHeaderFooterBackground.png", 63, 18.7, TextLabel);

		this.ImageHeader = ImageHeader;
		this.addChild(this.ImageHeader);

		const TextButtonCancelLabel = new UILabel("Cancel", 30, 0xA13000, 75, 30, );
		const ButtonCancel = new UIButtonTextButtonCancelLabelUILabel("images/PopupButton.png", 16, 16.5, TextButtonCancelLabel);
		const TextButtonOkLabel = new UILabel("Ok", 30, 0xA13000, 75, 30, );
		const ButtonOk = new UIButtonTextButtonOkLabelUILabel("images/PopupButton.png", 278, 16.5, TextButtonOkLabel);
		const ImageFooter = new UISpriteButtonCancelUIButtonButtonOkUIButton("images/PopupHeaderFooterBackground.png", 63, 296, ButtonCancel, ButtonOk);

		this.ImageFooter = ImageFooter;
		this.addChild(this.ImageFooter);
	};
}

class UISpriteImageBackgroundMessageUISprite extends UISprite {
	public ImageBackgroundMessage: UISpriteTextMessageUILabelTextTestUILabel;

	constructor(textureName: string, x: number, y: number, ImageBackgroundMessage: UISpriteTextMessageUILabelTextTestUILabel) {
		super(textureName, x, y);

		this.ImageBackgroundMessage = ImageBackgroundMessage;
		this.addChild(this.ImageBackgroundMessage);
	};
}

class UISpriteTextMessageUILabelTextTestUILabel extends UISprite {
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

class UISpriteTextLabelUILabel extends UISprite {
	public TextLabel: UILabel;

	constructor(textureName: string, x: number, y: number, TextLabel: UILabel) {
		super(textureName, x, y);

		this.TextLabel = TextLabel;
		this.addChild(this.TextLabel);
	};
}

class UISpriteButtonCancelUIButtonButtonOkUIButton extends UISprite {
	public ButtonCancel: UIButtonTextButtonCancelLabelUILabel;
	public ButtonOk: UIButtonTextButtonOkLabelUILabel;

	constructor(textureName: string, x: number, y: number, ButtonCancel: UIButtonTextButtonCancelLabelUILabel, ButtonOk: UIButtonTextButtonOkLabelUILabel) {
		super(textureName, x, y);

		this.ButtonCancel = ButtonCancel;
		this.addChild(this.ButtonCancel);

		this.ButtonOk = ButtonOk;
		this.addChild(this.ButtonOk);
	};
}

class UIButtonTextButtonCancelLabelUILabel extends UIButton {
	public TextButtonCancelLabel: UILabel;

	constructor(background: string, x: number, y: number, TextButtonCancelLabel: UILabel) {
		super(background, x, y);

		this.TextButtonCancelLabel = TextButtonCancelLabel;
		this.addChild(this.TextButtonCancelLabel);
	};
}

class UIButtonTextButtonOkLabelUILabel extends UIButton {
	public TextButtonOkLabel: UILabel;

	constructor(background: string, x: number, y: number, TextButtonOkLabel: UILabel) {
		super(background, x, y);

		this.TextButtonOkLabel = TextButtonOkLabel;
		this.addChild(this.TextButtonOkLabel);
	};
}
