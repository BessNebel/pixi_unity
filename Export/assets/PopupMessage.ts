import UISprite from './UI/UISprite';
import UIPopup from './UI/UIPopup';
import UILabel from './UI/UILabel';
import UIButton from './UI/UIButton';
import * as SC from './SupportClasses';

export default class PopupMessage extends UIPopup {
	public ImageBackground: SC.UISpriteImageBackgroundMessageUISprite;
	public ImageHeader: SC.UISpriteTextLabelUILabel;
	public ImageFooter: SC.UISpriteButtonCancelUIButtonButtonOkUIButton;

	constructor(x: number, y: number) {
		super(x, y);

		const TextMessage4 = new UILabel("Message", 35, 0x009C13, 120, 40, );
		const TextTest5 = new UILabel("Text", 35, 0xFF7200, 480, 110, );
		const ImageBackgroundMessage3 = new SC.UISpriteTextMessageUILabelTextTestUILabel("images/PopupContentBackground.png", 15, 127.5, TextMessage4, TextTest5);
		const ImageBackground2 = new SC.UISpriteImageBackgroundMessageUISprite("images/PopupBackground.png", 0, 0, ImageBackgroundMessage3);

		this.ImageBackground = ImageBackground2;
		this.addChild(this.ImageBackground);

		const TextLabel7 = new UILabel("Label", 50, 0xFFE700, 222, 46.50001, );
		const ImageHeader6 = new SC.UISpriteTextLabelUILabel("images/PopupHeaderFooterBackground.png", 63, 18.7, TextLabel7);

		this.ImageHeader = ImageHeader6;
		this.addChild(this.ImageHeader);

		const TextLabel10 = new UILabel("Cancel", 30, 0xA13000, 75, 30, );
		const ButtonCancel9 = new SC.UIButtonTextLabelUILabel("images/PopupButton.png", 16, 16.5, TextLabel10);
		const TextLabel12 = new UILabel("Ok", 30, 0xA13000, 75, 30, );
		const ButtonOk11 = new SC.UIButtonTextLabelUILabel("images/PopupButton.png", 278, 16.5, TextLabel12);
		const ImageFooter8 = new SC.UISpriteButtonCancelUIButtonButtonOkUIButton("images/PopupHeaderFooterBackground.png", 63, 296, ButtonCancel9, ButtonOk11);

		this.ImageFooter = ImageFooter8;
		this.addChild(this.ImageFooter);
	};
}
