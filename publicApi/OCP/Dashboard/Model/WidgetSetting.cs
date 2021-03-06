﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OCP.Dashboard.Model
{
/**
 * Interface WidgetSetting
 *
 * Each setting that can be edited by a user should be defined in a
 * WidgetSetting.
 *
 * When using this framework, the settings interface is generated by the
 * Dashboard app.
 *
 * Each WidgetSetting must be generated and declared in the WidgetTemplate
 * during the setup of the widget in the IDashboardWidget using addSetting().
 *
 * @see IDashboardWidget::getWidgetTemplate
 * @see WidgetTemplate::addSetting
 *
 * @since 15.0.0
 *
 * @package OCP\Dashboard\Model
 */
public sealed class WidgetSetting {


	const string TYPE_INPUT = "input";
	const string TYPE_CHECKBOX = "checkbox";


	/** @var string */
	private string name = "";

	/** @var string */
	private string title = "";

	/** @var string */
	private string type = "";

	/** @var string */
	private string placeholder = "";

	/** @var string */
	private string @default = "";


	/**
	 * WidgetSetting constructor.
	 *
	 * @since 15.0.0
	 *
	 * @param string type
	 */
	public WidgetSetting(string type = "") {
		this.type = type;
	}


	/**
	 * Set the name of the setting (full string, no space)
	 *
	 * @since 15.0.0
	 *
	 * @param string name
	 *
	 * @return WidgetSetting
	 */
	public WidgetSetting setName(string name) {
		this.name = name;

		return this;
	}

	/**
	 * Get the name of the setting
	 *
	 * @since 15.0.0
	 *
	 * @return string
	 */
	public string getName(){
		return this.name;
	}


	/**
	 * Set the title/display name of the setting.
	 *
	 * @since 15.0.0
	 *
	 * @param string title
	 *
	 * @return WidgetSetting
	 */
	public WidgetSetting setTitle(string title){
		this.title = title;

		return this;
	}

	/**
	 * Get the title of the setting
	 *
	 * @since 15.0.0
	 *
	 * @return string
	 */
	public string getTitle() {
		return this.title;
	}


	/**
	 * Set the type of the setting (input, checkbox, ...)
	 *
	 * @since 15.0.0
	 *
	 * @param string type
	 *
	 * @return WidgetSetting
	 */
	public WidgetSetting setType(string type) {
		this.type = type;

		return this;
	}

	/**
	 * Get the type of the setting.
	 *
	 * @since 15.0.0
	 *
	 * @return string
	 */
	public string getType(){
		return this.type;
	}


	/**
	 * Set the placeholder (in case of type=input)
	 *
	 * @since 15.0.0
	 *
	 * @param string text
	 *
	 * @return WidgetSetting
	 */
	public WidgetSetting setPlaceholder(string text) {
		this.placeholder = text;

		return this;
	}

	/**
	 * Get the placeholder.
	 *
	 * @since 15.0.0
	 *
	 * @return string
	 */
	public string getPlaceholder(){
		return this.placeholder;
	}


	/**
	 * Set the default value of the setting.
	 *
	 * @since 15.0.0
	 *
	 * @param string value
	 *
	 * @return WidgetSetting
	 */
	public WidgetSetting setDefault(string value) {		
        this.@default = value;

		return this;
	}

	/**
	 * Get the default value.
	 *
	 * @since 15.0.0
	 *
	 * @return string
	 */
	public string getDefault() {
		return this.@default;
	}


	///**
	// * @since 15.0.0
	// *
	// * @return array
	// */
	//public function jsonSerialize() {
	//	return [
	//		'name' => this.getName(),
	//		'title' => this.getTitle(),
	//		'type' => this.getTitle(),
	//		'default' => this.getDefault(),
	//		'placeholder' => this.getPlaceholder()
	//	];
	//}


}


}
