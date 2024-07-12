using System;
using System.Collections.Generic;

namespace Disney.MobileNetwork
{
	public class FireEventParameters
	{
		internal Dictionary<string, object> valuePayload;

		internal string eventName;

		internal Dictionary<FireEventType, string> eventNameList;

		public string checkoutAsGuest
		{
			set
			{
				valuePayload.Add("checkout_as_guest", value ?? "");
			}
		}

		public string contentId
		{
			set
			{
				valuePayload.Add("content_id", value ?? "");
			}
		}

		public string contentType
		{
			set
			{
				valuePayload.Add("content_type", value ?? "");
			}
		}

		public string currency
		{
			set
			{
				valuePayload.Add("currency", value ?? "");
			}
		}

		public string dateString
		{
			set
			{
				if (!valuePayload.ContainsKey("now_date"))
				{
					valuePayload.Add("now_date", value ?? "");
				}
			}
		}

		public DateTime date
		{
			set
			{
				if (!valuePayload.ContainsKey("now_date"))
				{
					valuePayload.Add("now_date", value.ToUniversalTime().ToString() ?? "");
				}
			}
		}

		public string description
		{
			set
			{
				valuePayload.Add("description", value ?? "");
			}
		}

		public string destination
		{
			set
			{
				valuePayload.Add("destination", value ?? "");
			}
		}

		public float duration
		{
			set
			{
				valuePayload.Add("duration", value);
			}
		}

		public string endDateString
		{
			set
			{
				if (!valuePayload.ContainsKey("end_date"))
				{
					valuePayload.Add("end_date", value ?? "");
				}
			}
		}

		public DateTime endDate
		{
			set
			{
				if (!valuePayload.ContainsKey("end_date"))
				{
					valuePayload.Add("end_date", value.ToUniversalTime().ToString() ?? "");
				}
			}
		}

		public string itemAddedFrom
		{
			set
			{
				valuePayload.Add("item_added_from", value ?? "");
			}
		}

		public string level
		{
			set
			{
				valuePayload.Add("level", value ?? "");
			}
		}

		public float maxRatingValue
		{
			set
			{
				valuePayload.Add("max_rating_value", value);
			}
		}

		public string name
		{
			set
			{
				valuePayload.Add("name", value ?? "");
			}
		}

		public string orderId
		{
			set
			{
				valuePayload.Add("order_id", value ?? "");
			}
		}

		public string origin
		{
			set
			{
				valuePayload.Add("origin", value ?? "");
			}
		}

		public float price
		{
			set
			{
				valuePayload.Add("price", value);
			}
		}

		public string quantity
		{
			set
			{
				valuePayload.Add("quantity", value ?? "");
			}
		}

		public float ratingValue
		{
			set
			{
				valuePayload.Add("rating_value", value);
			}
		}

		public string receiptId
		{
			set
			{
				valuePayload.Add("receipt_id", value ?? "");
			}
		}

		public string referralFrom
		{
			set
			{
				valuePayload.Add("referral_from", value ?? "");
			}
		}

		public string registrationMethod
		{
			set
			{
				valuePayload.Add("registration_method", value ?? "");
			}
		}

		public string results
		{
			set
			{
				valuePayload.Add("results", value ?? "");
			}
		}

		public string score
		{
			set
			{
				valuePayload.Add("score", value ?? "");
			}
		}

		public string searchTerm
		{
			set
			{
				valuePayload.Add("search_term", value ?? "");
			}
		}

		public string startDateString
		{
			set
			{
				if (!valuePayload.ContainsKey("start_date"))
				{
					valuePayload.Add("start_date", value ?? "");
				}
			}
		}

		public DateTime startDate
		{
			set
			{
				if (!valuePayload.ContainsKey("start_date"))
				{
					valuePayload.Add("start_date", value.ToUniversalTime().ToString() ?? "");
				}
			}
		}

		public string success
		{
			set
			{
				valuePayload.Add("success", value ?? "");
			}
		}

		public string userId
		{
			set
			{
				valuePayload.Add("user_id", value ?? "");
			}
		}

		public string userName
		{
			set
			{
				valuePayload.Add("user_name", value ?? "");
			}
		}

		public string validated
		{
			set
			{
				valuePayload.Add("validated", value ?? "");
			}
		}

		public FireEventParameters(FireEventType fireEventType)
		{
			valuePayload = new Dictionary<string, object>();
			eventNameList = new Dictionary<FireEventType, string>
			{
				{
					FireEventType.Achievement,
					"Achievement"
				},
				{
					FireEventType.AddToCart,
					"Add to Cart"
				},
				{
					FireEventType.AddToWishList,
					"Add to Wish List"
				},
				{
					FireEventType.CheckoutStart,
					"Checkout Start"
				},
				{
					FireEventType.LevelComplete,
					"Level Complete"
				},
				{
					FireEventType.Purchase,
					"Purchase"
				},
				{
					FireEventType.Rating,
					"Rating"
				},
				{
					FireEventType.RegistrationComplete,
					"Registration Complete"
				},
				{
					FireEventType.Search,
					"Search"
				},
				{
					FireEventType.TutorialComplete,
					"Tutorial Complete"
				},
				{
					FireEventType.View,
					"View"
				}
			};
			if (!eventNameList.TryGetValue(fireEventType, out eventName))
			{
				eventName = "";
			}
		}
	}
}
