﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamFeedBot.Rulesets
{
	public class RedRuleset : Ruleset
	{
		public RedRuleset(Memory memory, Settings settings)
			: base(memory, settings)
		{ }

		public override string? CalculateDeltas(RunStatus? status, RunStatus? oldStatus, out string? announcement)
		{
			StringBuilder builder = new StringBuilder();
			StringBuilder aBuilder = new StringBuilder();
			if (oldStatus == null || status == null)
			{
				announcement = null;
				return null; //calculate deltas between two statuses, not just one
			}

			if ((oldStatus.Name == null || oldStatus.Gender == null) && status.Name != null && status.Gender != null) //TODO: no gender in red
			{
				string choice = status.Gender == Gender.Female ? "girl" : "boy";

				builder.Append($"**We are a {choice} named {status.Name}!** ");
			}

			if (status.BattleKind != null && status.GameStats != null && oldStatus.GameStats != null &&
				status.GameStats.BattlesFought != oldStatus.GameStats.BattlesFought)
			{
				switch (status.BattleKind)
				{
					case BattleKind.Wild:
						if (status.EnemyParty != null && status.EnemyParty.Count > 0 && status.EnemyParty[0] != null)
						{
							string[] rand1 =
							{
								"come across", "run into", "step on", "stumble upon", "encounter", "bump into",
								"run across"
							};
							string[] rand2 =
								{"Facing off against", "Battling", "Grappling", "Confronted by", "Wrestling"};
							string[] rand3 =
							{
								"picks a fight with", "engages", "thinks it can take", "crashes into", "smacks into",
								"collides with", "jumps", "ambushes", "attacks", "assaults"
							};
							string[] choice =
							{
								$"We {rand1[Random.Next(rand1.Length)]} a wild {status.EnemyParty[0].Species!.Name}. ",
								$"{rand2[Random.Next(rand2.Length)]} a wild {status.EnemyParty[0].Species!.Name}. ",
								$"A wild {status.EnemyParty[0].Species!.Name} {rand3[Random.Next(rand3.Length)]} us. "
							};
							string message = choice[Random.Next(choice.Length)];
							builder.Append(message);
						}

						EnemyName = null;
						break;
					case BattleKind.Trainer:
						if (status.EnemyTrainers?[0] != null)
						{
							Trainer trainer = status.EnemyTrainers[0];
							/*if (SpecialClasses_USUM.Contains(trainer.ClassId))
							{
								builder.Append($"**VS {trainer.ClassName} {trainer.Name}!** ");
								if (Attempts.TryGetValue(trainer.Id, out int val))
								{
									builder.Append($"Attempt #{val + 1}! ");
									Attempts.Remove(trainer.Id);
									Attempts.Add(trainer.Id, val + 1);
								}
								else
								{
									Attempts.Add(trainer.Id, 1);
								}

								break;
							}*/ //TODO fix for red

							if (trainer.ClassId == -1)
							{
								string[] rand1 =
								{
										"come across", "run into", "step on", "stumble upon", "encounter", "bump into",
										"run across"
									};
								string[] rand2 =
									{"Facing off against", "Battling", "Grappling", "Confronted by", "Wrestling"};
								string[] rand3 =
								{
										"picks a fight with", "engages", "thinks it can take", "crashes into", "smacks into",
										"collides with", "jumps", "ambushes", "attacks", "assaults"
									};
								string[] choice =
								{
										$"We {rand1[Random.Next(rand1.Length)]} a wild {status.EnemyParty![0].Species!.Name}. ",
										$"{rand2[Random.Next(rand2.Length)]} a wild {status.EnemyParty[0].Species!.Name}. ",
										$"A wild {status.EnemyParty[0].Species!.Name} {rand3[Random.Next(rand3.Length)]} us. "
									};
								string message = choice[Random.Next(choice.Length)];
								builder.Append(message);
								EnemyName = null;
								break;
							}

							string[] c1 = { "fight", "battle", "face off against" };
							string[] c2 = { "cheeky", "rogue", "roving", "wandering" };
							string[] c3 = { " wandering", "n eager" };
							string[] choices =
							{
									$"We {c1[Random.Next(c1.Length)]} a {c2[Random.Next(c2.Length)]} {trainer.ClassName}, named {trainer.Name}{(status.EnemyParty.Any(x => x.Active == true) ? $", and their {string.Join(", ", status.EnemyParty.Where(x => x.Active == true).Select(x => x.Species?.Name ?? ""))}" : "")}. ",
									$"We get spotted by a{c3[Random.Next(c3.Length)]} {trainer.ClassName} named {trainer.Name}, and begin a battle{(status.EnemyParty.Any(x => x.Active == true) ? $" against their {string.Join(", ", status.EnemyParty.Where(x => x.Active == true).Select(x => x.Species?.Name ?? ""))}" : "")}. ",
									$"{trainer.ClassName} {trainer.Name} picks a fight with us{(status.EnemyParty.Any(x => x.Active == true) ? $", using their {string.Join(", ", status.EnemyParty.Where(x => x.Active == true).Select(x => x.Species?.Name ?? ""))}" : "")}. "
								};
							builder.Append(choices[Random.Next(choices.Length)]);
						}

						break;
				}
			}

			if (status.GameStats != null && oldStatus.GameStats != null && status.GameStats.Blackouts > oldStatus.GameStats.Blackouts)
			{
				string[] options = { "**BLACKED OUT!** ", "**We BLACK OUT!** ", "**BLACK OUT...** " };
				string message = options[Random.Next(options.Length)];
				builder.Append(message);
			}

			if (status.BattleKind == null && oldStatus!.BattleKind == BattleKind.Trainer &&
				status.GameStats?.Blackouts == oldStatus.GameStats?.Blackouts)
			{
				if (oldStatus.EnemyTrainers != null)
				{
					Trainer trainer = oldStatus.EnemyTrainers[0];
					/*if (SpecialClasses_USUM.Contains(trainer.ClassId))
					{
						builder.Append($"**Defeated {trainer.ClassName} {trainer.Name}!** ");
						EnemyName = trainer.ClassName + " " + trainer.Name;
					}

					if (Enumerable.Range(494, 3).Contains(trainer.Id))
					{
						builder.Append("**TEH URN!** ");
						if (!Memory.Urned)
						{
							aBuilder.Append($"**We defeated {trainer.ClassName} {trainer.Name}! TEH URN!** ");
							Memory.Urned = true;
						}
					}*/ //TODO fix for red
				}
			}

			List<uint> ids = new List<uint>();

			ItemEqualityComparer comparer = new ItemEqualityComparer();

			List<Item> distinctMedicine = new List<Item>();
			if (status.Items?.Medicine != null)
				distinctMedicine.AddRange(status.Items.Medicine);
			if (oldStatus!.Items?.Medicine != null)
				distinctMedicine.AddRange(oldStatus.Items.Medicine);
			distinctMedicine = distinctMedicine.Distinct(comparer).ToList();

			foreach (Item item in distinctMedicine)
			{
				if (ids.Contains(item.Id)) continue;
				long count = status?.Items?.Medicine?.Where(x => x.Id == item.Id)?.Sum(x => x.Count ?? 1) ?? 0;
				bool res = oldStatus?.Items?.Medicine?.FirstOrDefault(x => x.Id == item.Id) != null;
				if (res)
				{
					long? oldCount = oldStatus?.Items?.Medicine?.Where(x => x.Id == item.Id)?.Sum(x => x.Count ?? 1);
					count -= oldCount ?? 0;
				}

				if (count != 0)
				{
					Pokemon[] monsGive = status!.Party.Where(x => x.HeldItem != null && x.HeldItem.Id == item.Id)
						.Where(x =>
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							(oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem == null ||
							 oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem?.Id !=
							 x.HeldItem?.Id))
						.ToArray();
					Pokemon[] monsTake = status.Party.Where(x =>
							x.HeldItem == null ||
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							oldStatus.Party.First(y => x.PersonalityValue == y.PersonalityValue).HeldItem?.Id !=
							x.HeldItem.Id)
						.Where(x =>
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem != null
							&& oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem?.Id ==
							item.Id)
						.ToArray();
					List<Pokemon> monsGivePc = new List<Pokemon>();
					foreach (Box box in status.PC?.Boxes ?? new List<Box>())
					{
						Box? oldBox = oldStatus!.PC?.Boxes?.FirstOrDefault(x => x.BoxNumber == box.BoxNumber);
						if (oldBox != null)
						{
							monsGivePc.AddRange(box.BoxContents
								.Where(x => x.HeldItem != null && x.HeldItem.Id == item.Id)
								.Where(x => oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
											(oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
												 .HeldItem == null ||
											 oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
												 .HeldItem?.Id != x.HeldItem?.Id)).ToList());
						}
					}

					List<Pokemon> monsTakePc = new List<Pokemon>();
					foreach (Box box in status.PC?.Boxes ?? new List<Box>())
					{
						Box? oldBox = oldStatus!.PC?.Boxes?.FirstOrDefault(x => x.BoxNumber == box.BoxNumber);
						if (oldBox != null)
						{
							monsTakePc.AddRange(box.BoxContents
								.Where(x => x.HeldItem == null ||
											oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
											oldBox.BoxContents.First(y => x.PersonalityValue == y.PersonalityValue)
												.HeldItem?.Id != x.HeldItem?.Id).Where(x =>
									oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
									oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem !=
									null && oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
										.HeldItem?.Id == item.Id).ToList());
						}
					}

					if (monsGive.Length != 0)
					{
						foreach (Pokemon mon in monsGive)
						{
							builder.Append($"We give {mon.Name} ({mon.Species!.Name}) a {item.Name} to hold. ");
							count++;
						}
					}

					if (monsGivePc != null && monsGivePc.Count != 0)
					{
						foreach (Pokemon mon in monsGivePc)
						{
							builder.Append($"We give {mon.Name} ({mon.Species!.Name}) a {item.Name} to hold. ");
							count++;
						}
					}

					if (monsTake.Length != 0)
					{
						foreach (Pokemon mon in monsTake)
						{
							builder.Append($"We take a {item.Name} away from {mon.Name} ({mon.Species!.Name}). ");
							count--;
						}
					}

					if (monsTakePc != null && monsTakePc.Count != 0)
					{
						foreach (Pokemon mon in monsTakePc)
						{
							builder.Append($"We take a {item.Name} away from {mon.Name} ({mon.Species!.Name}). ");
							count--;
						}
					}

					if (status.BattleKind != null && count < 0)
						builder.Append(
							$"We use {(count == -1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
					else if (count < 0 && status.Money > oldStatus!.Money && oldStatus.BattleKind == null)
					{
						builder.Append(
							$"We sell {(count == 1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
					}
					else if (count < 0)
						builder.Append(
							$"We use {(count == -1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
					else if (count > 0 && status.Money < oldStatus!.Money)
					{
						builder.Append(
							$"We buy {(count == 1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
					}
					else if (count > 0)
						builder.Append(
							$"We pick up {(count == 1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
				}

				ids.Add(item.Id);
			}

			List<Item> distinctBerries = new List<Item>();
			if (status?.Items?.Berries != null)
				distinctBerries.AddRange(status.Items.Berries);
			if (oldStatus?.Items?.Berries != null)
				distinctBerries.AddRange(oldStatus.Items.Berries);
			distinctBerries = distinctBerries.Distinct(comparer).ToList();

			foreach (Item item in distinctBerries)
			{
				if (ids.Contains(item.Id)) continue;
				long count = status?.Items?.Berries?.Where(x => x.Id == item.Id)?.Sum(x => x.Count ?? 1) ?? 0;
				bool res = oldStatus?.Items?.Berries?.FirstOrDefault(x => x.Id == item.Id) != null;
				if (res)
				{
					long? oldCount = oldStatus?.Items?.Berries?.Where(x => x.Id == item.Id)?.Sum(x => x.Count ?? 1);
					count -= oldCount ?? 0;
				}

				if (count != 0)
				{
					Pokemon[] monsGive = status!.Party.Where(x => x.HeldItem != null && x.HeldItem.Id == item.Id)
						.Where(x =>
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							(oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem == null ||
							 oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem?.Id !=
							 x.HeldItem?.Id))
						.ToArray();
					Pokemon[] monsTake = status.Party.Where(x =>
							x.HeldItem == null ||
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							oldStatus.Party.First(y => x.PersonalityValue == y.PersonalityValue).HeldItem?.Id !=
							x.HeldItem.Id)
						.Where(x =>
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem != null
							&& oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem?.Id ==
							item.Id)
						.ToArray();
					List<Pokemon> monsGivePc = new List<Pokemon>();
					foreach (Box box in status.PC?.Boxes ?? new List<Box>())
					{
						Box? oldBox = oldStatus!.PC?.Boxes?.FirstOrDefault(x => x.BoxNumber == box.BoxNumber);
						if (oldBox != null)
						{
							monsGivePc.AddRange(box.BoxContents
								.Where(x => x.HeldItem != null && x.HeldItem.Id == item.Id)
								.Where(x => oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
											(oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
												 .HeldItem == null ||
											 oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
												 .HeldItem?.Id != x.HeldItem?.Id)).ToList());
						}
					}

					List<Pokemon> monsTakePc = new List<Pokemon>();
					foreach (Box box in status.PC?.Boxes ?? new List<Box>())
					{
						Box? oldBox = oldStatus!.PC?.Boxes?.FirstOrDefault(x => x.BoxNumber == box.BoxNumber);
						if (oldBox != null)
						{
							monsTakePc.AddRange(box.BoxContents
								.Where(x => x.HeldItem == null ||
											oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
											oldBox.BoxContents.First(y => x.PersonalityValue == y.PersonalityValue)
												.HeldItem?.Id != x.HeldItem?.Id).Where(x =>
									oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
									oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem !=
									null && oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
										.HeldItem?.Id == item.Id).ToList());
						}
					}
					if (monsGive.Length != 0)
					{
						foreach (Pokemon mon in monsGive)
						{
							builder.Append($"We give {mon.Name} ({mon.Species!.Name}) a {item.Name} to hold. ");
							count++;
						}
					}

					if (monsGivePc != null && monsGivePc.Count != 0)
					{
						foreach (Pokemon mon in monsGivePc)
						{
							builder.Append($"We give {mon.Name} ({mon.Species!.Name}) a {item.Name} to hold. ");
							count++;
						}
					}

					if (monsTake.Length != 0)
					{
						foreach (Pokemon mon in monsTake)
						{
							builder.Append($"We take a {item.Name} away from {mon.Name} ({mon.Species!.Name}). ");
							count--;
						}
					}

					if (monsTakePc != null && monsTakePc.Count != 0)
					{
						foreach (Pokemon mon in monsTakePc)
						{
							builder.Append($"We take a {item.Name} away from {mon.Name} ({mon.Species!.Name}). ");
							count--;
						}
					}

					if (status.BattleKind != null && count < 0)
						builder.Append(
							$"We use {(count == -1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
					else if (count < 0 && status.Money > oldStatus!.Money && oldStatus.BattleKind == null)
					{
						builder.Append(
							$"We sell {(count == 1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
					}
					else if (count < 0)
						builder.Append(
							$"We use {(count == -1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
					else if (count > 0 && status.Money < oldStatus!.Money)
					{
						builder.Append(
							$"We buy {(count == 1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
					}
					else if (count > 0)
						builder.Append(
							$"We pick up {(count == 1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
				}

				ids.Add(item.Id);
			}

			List<Item> distinctItems = new List<Item>();
			if (status?.Items?.Items != null)
				distinctItems.AddRange(status.Items.Items);
			if (oldStatus?.Items?.Items != null)
				distinctItems.AddRange(oldStatus.Items.Items);
			distinctItems = distinctItems.Distinct(comparer).ToList();

			foreach (Item item in distinctItems)
			{
				if (ids.Contains(item.Id)) continue;
				long count = status?.Items?.Items?.Where(x => x.Id == item.Id)?.Sum(x => x.Count ?? 1) ?? 0;
				bool res = oldStatus?.Items?.Items?.FirstOrDefault(x => x.Id == item.Id) != null;
				if (res)
				{
					long? oldCount = oldStatus?.Items?.Items?.Where(x => x.Id == item.Id)?.Sum(x => x.Count ?? 1);
					count -= oldCount ?? 0;
				}

				if (count != 0)
				{
					Pokemon[] monsGive = status!.Party.Where(x => x.HeldItem != null && x.HeldItem.Id == item.Id)
						.Where(x =>
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							(oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem == null ||
							 oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem?.Id !=
							 x.HeldItem?.Id))
						.ToArray();
					Pokemon[] monsTake = status.Party.Where(x =>
							x.HeldItem == null ||
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							oldStatus.Party.First(y => x.PersonalityValue == y.PersonalityValue).HeldItem?.Id !=
							x.HeldItem.Id)
						.Where(x =>
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem != null
							&& oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem?.Id ==
							item.Id)
						.ToArray();
					List<Pokemon> monsGivePc = new List<Pokemon>();
					foreach (Box box in status.PC?.Boxes ?? new List<Box>())
					{
						Box? oldBox = oldStatus!.PC?.Boxes?.FirstOrDefault(x => x.BoxNumber == box.BoxNumber);
						if (oldBox != null)
						{
							monsGivePc.AddRange(box.BoxContents
								.Where(x => x.HeldItem != null && x.HeldItem.Id == item.Id)
								.Where(x => oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
											(oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
												 .HeldItem == null ||
											 oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
												 .HeldItem?.Id != x.HeldItem?.Id)).ToList());
						}
					}

					List<Pokemon> monsTakePc = new List<Pokemon>();
					foreach (Box box in status.PC?.Boxes ?? new List<Box>())
					{
						Box? oldBox = oldStatus!.PC?.Boxes?.FirstOrDefault(x => x.BoxNumber == box.BoxNumber);
						if (oldBox != null)
						{
							monsTakePc.AddRange(box.BoxContents
								.Where(x => x.HeldItem == null ||
											oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
											oldBox.BoxContents.First(y => x.PersonalityValue == y.PersonalityValue)
												.HeldItem?.Id != x.HeldItem?.Id).Where(x =>
									oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
									oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem !=
									null && oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
										.HeldItem?.Id == item.Id).ToList());
						}
					}
					if (monsGive.Length != 0)
					{
						foreach (Pokemon mon in monsGive)
						{
							builder.Append($"We give {mon.Name} ({mon.Species!.Name}) a {item.Name} to hold. ");
							count++;
						}
					}

					if (monsGivePc != null && monsGivePc.Count != 0)
					{
						foreach (Pokemon mon in monsGivePc)
						{
							builder.Append($"We give {mon.Name} ({mon.Species!.Name}) a {item.Name} to hold. ");
							count++;
						}
					}

					if (monsTake.Length != 0)
					{
						foreach (Pokemon mon in monsTake)
						{
							builder.Append($"We take a {item.Name} away from {mon.Name} ({mon.Species!.Name}). ");
							count--;
						}
					}

					if (monsTakePc != null && monsTakePc.Count != 0)
					{
						foreach (Pokemon mon in monsTakePc)
						{
							builder.Append($"We take a {item.Name} away from {mon.Name} ({mon.Species!.Name}). ");
							count--;
						}
					}

					if (status.BattleKind == BattleKind.Wild && status.EnemyParty != null && status.EnemyParty.Count > 0 && count < 0 /*&& BallIds.Contains(item.Id)*/) //todo fix for red
						builder.Append(
							$"We throw {(count == -1 ? $"a {item.Name}" : $"some {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")} at the wild {status.EnemyParty[0].Species!.Name}. ");
					else if (status.BattleKind == BattleKind.Trainer && status.EnemyParty != null && status.EnemyParty.Count > 0 && count < 0 /*&& BallIds.Contains(item.Id)*/) //todo fix for red
						builder.Append(
							$"We throw {(count == -1 ? $"a {item.Name}" : $"some {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")} at the opponent's {status.EnemyParty[0].Species!.Name}. ");
					else if (count < 0 && status.Money > oldStatus!.Money && oldStatus.BattleKind == null)
					{
						builder.Append(
							$"We sell {(count == 1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
					}
					else if (count < 0)
						builder.Append(
							$"We use {(count == -1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
					else if (count > 0 && status.Money < oldStatus!.Money)
					{
						builder.Append(
							$"We buy {(count == 1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
					}
					else if (count > 0)
						builder.Append(
							$"We pick up {(count == 1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
				}

				ids.Add(item.Id);
			}

			List<Item> distinctKey = new List<Item>();
			if (status?.Items?.Key != null)
				distinctKey.AddRange(status.Items.Key);
			if (oldStatus?.Items?.Key != null)
				distinctKey.AddRange(oldStatus.Items.Key);
			distinctKey = distinctKey.Distinct(comparer).ToList();

			foreach (Item item in distinctKey)
			{
				if (ids.Contains(item.Id)) continue;
				long count = status?.Items?.Key?.Where(x => x.Id == item.Id)?.Sum(x => x.Count ?? 1) ?? 0;
				bool res = oldStatus?.Items?.Key?.FirstOrDefault(x => x.Id == item.Id) != null;
				if (res)
				{
					long? oldCount = oldStatus?.Items?.Key?.Where(x => x.Id == item.Id)?.Sum(x => x.Count ?? 1);
					count -= oldCount ?? 0;
				}

				if (count != 0)
				{
					Pokemon[] monsGive = status!.Party.Where(x => x.HeldItem != null && x.HeldItem.Id == item.Id)
						.Where(x =>
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							(oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem == null ||
							 oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem?.Id !=
							 x.HeldItem?.Id))
						.ToArray();
					Pokemon[] monsTake = status.Party.Where(x =>
							x.HeldItem == null ||
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							oldStatus.Party.First(y => x.PersonalityValue == y.PersonalityValue).HeldItem?.Id !=
							x.HeldItem.Id)
						.Where(x =>
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem != null
							&& oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem?.Id ==
							item.Id)
						.ToArray();
					List<Pokemon> monsGivePc = new List<Pokemon>();
					foreach (Box box in status.PC?.Boxes ?? new List<Box>())
					{
						Box? oldBox = oldStatus!.PC?.Boxes?.FirstOrDefault(x => x.BoxNumber == box.BoxNumber);
						if (oldBox != null)
						{
							monsGivePc.AddRange(box.BoxContents
								.Where(x => x.HeldItem != null && x.HeldItem.Id == item.Id)
								.Where(x => oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
											(oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
												 .HeldItem == null ||
											 oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
												 .HeldItem?.Id != x.HeldItem?.Id)).ToList());
						}
					}

					List<Pokemon> monsTakePc = new List<Pokemon>();
					foreach (Box box in status.PC?.Boxes ?? new List<Box>())
					{
						Box? oldBox = oldStatus!.PC?.Boxes?.FirstOrDefault(x => x.BoxNumber == box.BoxNumber);
						if (oldBox != null)
						{
							monsTakePc.AddRange(box.BoxContents
								.Where(x => x.HeldItem == null ||
											oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
											oldBox.BoxContents.First(y => x.PersonalityValue == y.PersonalityValue)
												.HeldItem?.Id != x.HeldItem?.Id).Where(x =>
									oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
									oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem !=
									null && oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
										.HeldItem?.Id == item.Id).ToList());
						}
					}
					if (monsGive.Length != 0)
					{
						foreach (Pokemon mon in monsGive)
						{
							builder.Append($"We give {mon.Name} ({mon.Species!.Name}) a {item.Name} to hold. ");
							count++;
						}
					}

					if (monsGivePc != null && monsGivePc.Count != 0)
					{
						foreach (Pokemon mon in monsGivePc)
						{
							builder.Append($"We give {mon.Name} ({mon.Species!.Name}) a {item.Name} to hold. ");
							count++;
						}
					}

					if (monsTake.Length != 0)
					{
						foreach (Pokemon mon in monsTake)
						{
							builder.Append($"We take a {item.Name} away from {mon.Name} ({mon.Species!.Name}). ");
							count--;
						}
					}

					if (monsTakePc != null && monsTakePc.Count != 0)
					{
						foreach (Pokemon mon in monsTakePc)
						{
							builder.Append($"We take a {item.Name} away from {mon.Name} ({mon.Species!.Name}). ");
							count--;
						}
					}

					if (count < 0)
						builder.Append(
							$"We use {(count == -1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
					else if (count > 0)
						builder.Append(
							$"We pick up {(count == 1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
				}

				ids.Add(item.Id);
			}

			List<Item> distinctTMs = new List<Item>();
			if (status?.Items?.TMs != null)
				distinctTMs.AddRange(status.Items.TMs);
			if (oldStatus?.Items?.TMs != null)
				distinctTMs.AddRange(oldStatus.Items.TMs);
			distinctTMs = distinctTMs.Distinct(comparer).ToList();

			foreach (Item item in distinctTMs)
			{
				if (ids.Contains(item.Id)) continue;
				long count = status?.Items?.TMs?.Where(x => x.Id == item.Id)?.Sum(x => x.Count ?? 1) ?? 0;
				bool res = oldStatus?.Items?.TMs?.FirstOrDefault(x => x.Id == item.Id) != null;
				if (res)
				{
					long? oldCount = oldStatus?.Items?.TMs?.Where(x => x.Id == item.Id)?.Sum(x => x.Count ?? 1);
					count -= oldCount ?? 0;
				}

				if (count != 0)
				{
					Pokemon[] monsGive = status!.Party.Where(x => x.HeldItem != null && x.HeldItem.Id == item.Id)
						.Where(x =>
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							(oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem == null ||
							 oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem?.Id !=
							 x.HeldItem?.Id))
						.ToArray();
					Pokemon[] monsTake = status.Party.Where(x =>
							x.HeldItem == null ||
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							oldStatus.Party.First(y => x.PersonalityValue == y.PersonalityValue).HeldItem?.Id !=
							x.HeldItem.Id)
						.Where(x =>
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem != null
							&& oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem?.Id ==
							item.Id)
						.ToArray();
					List<Pokemon> monsGivePc = new List<Pokemon>();
					foreach (Box box in status.PC?.Boxes ?? new List<Box>())
					{
						Box? oldBox = oldStatus!.PC?.Boxes?.FirstOrDefault(x => x.BoxNumber == box.BoxNumber);
						if (oldBox != null)
						{
							monsGivePc.AddRange(box.BoxContents
								.Where(x => x.HeldItem != null && x.HeldItem.Id == item.Id)
								.Where(x => oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
											(oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
												 .HeldItem == null ||
											 oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
												 .HeldItem?.Id != x.HeldItem?.Id)).ToList());
						}
					}

					List<Pokemon> monsTakePc = new List<Pokemon>();
					foreach (Box box in status.PC?.Boxes ?? new List<Box>())
					{
						Box? oldBox = oldStatus!.PC?.Boxes?.FirstOrDefault(x => x.BoxNumber == box.BoxNumber);
						if (oldBox != null)
						{
							monsTakePc.AddRange(box.BoxContents
								.Where(x => x.HeldItem == null ||
											oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
											oldBox.BoxContents.First(y => x.PersonalityValue == y.PersonalityValue)
												.HeldItem?.Id != x.HeldItem?.Id).Where(x =>
									oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
									oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem !=
									null && oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
										.HeldItem?.Id == item.Id).ToList());
						}
					}
					if (monsGive.Length != 0)
					{
						foreach (Pokemon mon in monsGive)
						{
							builder.Append($"We give {mon.Name} ({mon.Species!.Name}) a {item.Name} to hold. ");
							count++;
						}
					}

					if (monsGivePc != null && monsGivePc.Count != 0)
					{
						foreach (Pokemon mon in monsGivePc)
						{
							builder.Append($"We give {mon.Name} ({mon.Species!.Name}) a {item.Name} to hold. ");
							count++;
						}
					}

					if (monsTake.Length != 0)
					{
						foreach (Pokemon mon in monsTake)
						{
							builder.Append($"We take a {item.Name} away from {mon.Name} ({mon.Species!.Name}). ");
							count--;
						}
					}

					if (monsTakePc != null && monsTakePc.Count != 0)
					{
						foreach (Pokemon mon in monsTakePc)
						{
							builder.Append($"We take a {item.Name} away from {mon.Name} ({mon.Species!.Name}). ");
							count--;
						}
					}

					if (count < 0 && status.Money > oldStatus!.Money && oldStatus.BattleKind == null)
					{
						builder.Append(
							$"We sell {(count == 1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
					}
					else if (count < 0)
						builder.Append(
							$"We use {(count == -1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
					else if (count > 0 && status.Money < oldStatus!.Money)
					{
						builder.Append(
							$"We buy {(count == 1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
					}
					else if (count > 0)
						builder.Append(
							$"We pick up {(count == 1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
				}

				ids.Add(item.Id);
			}

			List<Item> distinctZCrystals = new List<Item>();
			if (status?.Items?.ZCrystals != null)
				distinctZCrystals.AddRange(status.Items.ZCrystals);
			if (oldStatus?.Items?.ZCrystals != null)
				distinctZCrystals.AddRange(oldStatus.Items.ZCrystals);
			distinctZCrystals = distinctZCrystals.Distinct(comparer).ToList();

			foreach (Item item in distinctZCrystals) //TODO change to badges
			{
				/*
				if (ids.Contains(item.Id)) continue;
				long count = status?.Items?.ZCrystals?.Where(x => x.Id == item.Id)?.Sum(x => x.Count ?? 1) ?? 0;
				count += status?.Items?.FreeSpace?.Where(x => x.Id == item.Id)?.Sum(x => x.Count ?? 1) ?? 0;
				bool res = oldStatus?.Items?.ZCrystals?.FirstOrDefault(x => x.Id == item.Id) != null;
				res |= oldStatus?.Items?.FreeSpace?.FirstOrDefault(x => x.Id == item.Id) != null;
				if (res)
				{
					long? oldCount = oldStatus?.Items?.ZCrystals?.Where(x => x.Id == item.Id)?.Sum(x => x.Count ?? 1);
					if (oldCount != null)
						oldCount += oldStatus?.Items?.FreeSpace?.Where(x => x.Id == item.Id)?.Sum(x => x.Count ?? 1);
					else
						oldCount = oldStatus?.Items?.FreeSpace?.Where(x => x.Id == item.Id)?.Sum(x => x.Count ?? 1);
					count -= oldCount ?? 0;
				}

				if (count != 0)
				{
					Pokemon[] monsGive = status!.Party.Where(x => x.HeldItem != null && x.HeldItem.Id == item.Id)
						.Where(x =>
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							(oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem == null ||
							 oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem?.Id !=
							 x.HeldItem?.Id))
						.ToArray();
					Pokemon[] monsTake = status.Party.Where(x =>
							x.HeldItem == null ||
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							oldStatus.Party.First(y => x.PersonalityValue == y.PersonalityValue).HeldItem?.Id !=
							x.HeldItem.Id)
						.Where(x =>
							oldStatus!.Party.Any(y => x.PersonalityValue == y.PersonalityValue) &&
							oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem != null
							&& oldStatus.Party.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem?.Id ==
							item.Id)
						.ToArray();
					List<Pokemon> monsGivePc = new List<Pokemon>();
					foreach (Box box in status.PC?.Boxes ?? new List<Box>())
					{
						Box? oldBox = oldStatus!.PC?.Boxes?.FirstOrDefault(x => x.BoxNumber == box.BoxNumber);
						if (oldBox != null)
						{
							monsGivePc.AddRange(box.BoxContents
								.Where(x => x.HeldItem != null && x.HeldItem.Id == item.Id)
								.Where(x => oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
											(oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
												 .HeldItem == null ||
											 oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
												 .HeldItem?.Id != x.HeldItem?.Id)).ToList());
						}
					}

					List<Pokemon> monsTakePc = new List<Pokemon>();
					foreach (Box box in status.PC?.Boxes ?? new List<Box>())
					{
						Box? oldBox = oldStatus!.PC?.Boxes?.FirstOrDefault(x => x.BoxNumber == box.BoxNumber);
						if (oldBox != null)
						{
							monsTakePc.AddRange(box.BoxContents
								.Where(x => x.HeldItem == null ||
											oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
											oldBox.BoxContents.First(y => x.PersonalityValue == y.PersonalityValue)
												.HeldItem?.Id != x.HeldItem?.Id).Where(x =>
									oldBox.BoxContents.Any(y => x.PersonalityValue == y.PersonalityValue) &&
									oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue).HeldItem !=
									null && oldBox.BoxContents.First(y => y.PersonalityValue == x.PersonalityValue)
										.HeldItem?.Id == item.Id).ToList());
						}
					}
					if (monsGive.Length != 0)
					{
						foreach (Pokemon mon in monsGive)
						{
							builder.Append($"We give {mon.Name} ({mon.Species!.Name}) a {item.Name} to hold. ");
							count++;
						}
					}

					if (monsGivePc != null && monsGivePc.Count != 0)
					{
						foreach (Pokemon mon in monsGivePc)
						{
							builder.Append($"We give {mon.Name} ({mon.Species!.Name}) a {item.Name} to hold. ");
							count++;
						}
					}

					if (monsTake.Length != 0)
					{
						foreach (Pokemon mon in monsTake)
						{
							builder.Append($"We take a {item.Name} away from {mon.Name} ({mon.Species!.Name}). ");
							count--;
						}
					}

					if (monsTakePc != null && monsTakePc.Count != 0)
					{
						foreach (Pokemon mon in monsTakePc)
						{
							builder.Append($"We take a {item.Name} away from {mon.Name} ({mon.Species!.Name}). ");
							count--;
						}
					}

					if (count < 0)
						builder.Append(
							$"We use {(count == -1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}. ");
					else if (count > 0)
					{
						builder.Append(
							$"**We receive {(count == 1 ? $"a {item.Name}" : $"{Math.Abs(count)} {(item.Name?.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) == true ? item.Name : item.Name + "s")}")}!** ");
						if (!Memory.AnnouncedBadges.Contains(item.Id))
						{
							if (oldStatus!.BattleKind == BattleKind.Trainer && ImportantCrystalIds.Contains(item.Id))
							{
								aBuilder.Append(
									$"**We defeated {oldStatus.EnemyTrainers![0].ClassName} {oldStatus.EnemyTrainers[0].Name} and received the {item.Name} crystal!** ");
								Memory.AnnouncedBadges.Add(item.Id);
							}
							else if (ImportantCrystalIds.Contains(item.Id) && EnemyName != null)
							{
								aBuilder.Append(
									$"**We defeated {EnemyName} and received the {item.Name} crystal!** ");
								EnemyName = null;
								Memory.AnnouncedBadges.Add(item.Id);
							}
							else if (ImportantCrystalIds.Contains(item.Id))
							{
								aBuilder.Append(
									$"**We received the {item.Name} crystal!** ");
								Memory.AnnouncedBadges.Add(item.Id);
							}
						}
					}
				}

				ids.Add(item.Id);
				*/
			}

			if ((status!.Money < oldStatus!.Money || status.Money > oldStatus.Money) && oldStatus.BattleKind == null)
				builder.Append($"We have ₽{status.Money} left. ");

			if (status.Party != null)
			{
				for (int i = 0; i < status.Party.Count; i++)
				{
					Pokemon? oldMon = i >= oldStatus?.Party?.Count ? null : oldStatus?.Party?[i];
					if (oldMon == null) continue;
					uint pv = oldMon.PersonalityValue;
					Pokemon? mon = status.Party.FirstOrDefault(x => x.PersonalityValue == pv);
					Pokemon? oldBattleMon = null;
					if (oldStatus!.BattleParty != null)
					{
						oldBattleMon = oldStatus.BattleParty.Where(x => x != null).FirstOrDefault(x => x.PersonalityValue == pv);
					}

					if (mon == null)
					{
						continue;
					}

					if (mon.Level != oldMon.Level && mon.Level != oldBattleMon?.Level)
					{
						string[] choices =
						{
							$"**{oldMon.Name} ({oldMon.Species!.Name}) has grown to level {mon.Level}!** ",
							$"**{oldMon.Name} ({oldMon.Species!.Name}) is now level {mon.Level}!** ",
							$"**{oldMon.Name} ({oldMon.Species!.Name}) has leveled up to level {mon.Level}!** "
						};
						string message = choices[Random.Next(choices.Length)];
						builder.Append(message);
					}

					if (mon.Species!.Id != oldMon.Species!.Id && mon.Species.Id != oldBattleMon?.Species?.Id)
					{
						string[] choices =
						{
							$"**{oldMon.Name} ({oldMon.Species.Name}) has evolved into a {mon.Species.Name}! **",
							$"**{oldMon.Name} ({oldMon.Species.Name}) evolves into a {mon.Species.Name}! **"
						};
						string message = choices[Random.Next(choices.Length)];
						builder.Append(message);
					}
				}
			}

			if (status.BattleParty != null && oldStatus!.BattleParty != null)
			{
				for (int i = 0; i < status.BattleParty.Count; i++)
				{
					Pokemon? oldMon = i >= oldStatus?.BattleParty?.Count ? null : oldStatus?.BattleParty?[i];
					if (oldMon == null) continue;
					uint pv = oldMon.PersonalityValue;
					Pokemon? mon = status.BattleParty.Where(x => x != null).FirstOrDefault(x => x.PersonalityValue == pv);
					Pokemon? partyMon = status.Party.Where(x => x != null).FirstOrDefault(x => x.PersonalityValue == pv);
					if (mon == null)
					{
						continue;
					}

					if (mon.Level != oldMon.Level && mon.Level != partyMon.Level)
					{
						string[] choices =
						{
							$"**{oldMon.Name} ({oldMon.Species!.Name}) has grown to level {mon.Level}!** ",
							$"**{oldMon.Name} ({oldMon.Species.Name}) is now level {mon.Level}!** ",
							$"**{oldMon.Name} ({oldMon.Species.Name}) has leveled up to {mon.Level}!** "
						};
						string message = choices[Random.Next(choices.Length)];
						builder.Append(message);
					}

					if (mon.Species!.Id != oldMon.Species!.Id && mon.Species.Id != partyMon?.Species?.Id)
					{
						string[] choices =
						{
							$"**{oldMon.Name} ({oldMon.Species.Name}) has evolved into a {mon.Species.Name}! **",
							$"**{oldMon.Name} ({oldMon.Species.Name}) evolves into a {mon.Species.Name}! **"
						};
						string message = choices[Random.Next(choices.Length)];
						builder.Append(message);
					}
				}
			}

			bool flag = false;
			if (status.Party != null)
			{
				foreach (Pokemon mon in status.Party)
				{
					if (mon == null) continue;
					uint pv = mon.PersonalityValue;
					Pokemon? oldMon = oldStatus?.Party?.Where(x => x != null)?.FirstOrDefault(x => x.PersonalityValue == pv);
					Pokemon? oldBattleMon = null;
					if (oldStatus?.BattleParty != null)
					{
						oldBattleMon = oldStatus.BattleParty.Where(x => x != null).FirstOrDefault(x => x.PersonalityValue == pv);
					}

					if (oldMon == null) continue;
					foreach (Move? move in mon.Moves!)
					{
						if (move == null) continue;
						if (!oldMon.Moves.Where(x => x != null).Select(x => x.Id).Contains(move.Id) &&
							(oldBattleMon == null || !oldBattleMon.Moves.Where(x => x != null).Select(x => x.Id)
								 .Contains(move.Id)))
						{
							if (oldMon.Moves!.Count == 4 || (oldBattleMon != null && oldBattleMon.Moves!.Count == 4))
							{
								Move? oldMove;
								if (!oldMon.Moves.Where(x => x != null).Select(x => x.Id).Contains(move.Id) &&
									oldMon.Moves.Count == 4)
								{
									oldMove = oldMon.Moves.First(x => !mon.Moves.Contains(x));
								}
								else
									oldMove = oldBattleMon?.Moves?.FirstOrDefault(x => !mon.Moves.Contains(x));

								builder.Append(
									oldMove != null
										? $"**{mon.Name} ({mon.Species!.Name}) learned {move.Name} over {oldMove.Name}!** "
										: $"**{mon.Name} ({mon.Species!.Name}) learned {move.Name}!** ");
							}
							else
							{
								builder.Append($"**{mon.Name} ({mon.Species!.Name}) learned {move.Name}!** ");
							}
						}
					}

					if (mon.Health![0] == 0 && oldMon.Health![0] != 0 && oldBattleMon?.Health?[0] != 0)
					{
						string[] choice =
						{
							$"**{oldMon.Name} ({oldMon.Species!.Name}) has fainted!** ",
							$"**{oldMon.Name} ({oldMon.Species.Name}) has fallen!** ",
						};
						builder.Append(choice[Random.Next(choice.Length)]);
						flag = true;
					}
				}
			}

			if (status.BattleParty != null && oldStatus!.BattleParty != null)
			{
				foreach (Pokemon mon in status.BattleParty)
				{
					if (mon == null) continue;
					uint pv = mon.PersonalityValue;

					Pokemon oldMon = oldStatus.BattleParty.Where(x => x != null).FirstOrDefault(x => x.PersonalityValue == pv);
					if (oldMon == null) continue;
					foreach (Move? move in mon.Moves!)
					{
						if (move == null) continue;
						if (!oldMon.Moves.Where(x => x != null).Select(x => x.Id).Contains(move.Id))
						{
							if (oldMon.Moves!.Count == 4)
							{
								Move oldMove = oldMon.Moves.First(x => !mon.Moves.Contains(x));

								builder.Append(
									$"**{mon.Name} ({mon.Species!.Name}) learned {move.Name} over {oldMove.Name}!** ");
							}
							else
							{
								builder.Append($"**{mon.Name} ({mon.Species!.Name}) learned {move.Name}!** ");
							}
						}
					}

					if (mon.Health![0] == 0 && oldMon.Health![0] != 0 && !flag)
					{
						string[] choice =
						{
							$"**{oldMon.Name} ({oldMon.Species!.Name}) has fainted!** ",
							$"**{oldMon.Name} ({oldMon.Species.Name}) has fallen!** ",
						};
						builder.Append(choice[Random.Next(choice.Length)]);
					}
				}
			}

			if (status.Daycare != null)
			{
				foreach (Pokemon mon in status.Daycare)
				{
					if (oldStatus?.Daycare?.Any(x => x.PersonalityValue == mon.PersonalityValue) == true)
						continue;

					string[] choices =
					{
						$"**We drop {mon.Name} ({mon.Species!.Name}) off at the daycare.** ",
						$"**We put {mon.Name} ({mon.Species.Name}) into the daycare.** ",
						$"**We dump {mon.Name} ({mon.Species.Name}) into the daycare.** ",
						$"**We leave {mon.Name} ({mon.Species.Name} at the daycare centre.** "
					};
					builder.Append(choices[Random.Next(choices.Length)]);
				}
			}

			if (oldStatus?.Daycare != null)
			{
				foreach (Pokemon mon in oldStatus.Daycare)
				{
					if (status.Daycare.Any(x => x.PersonalityValue == mon.PersonalityValue))
						continue;

					string[] choices =
					{
						$"**We take {mon.Name} ({mon.Species!.Name}) back from the daycare.** ",
						$"**We pick up {mon.Name} ({mon.Species.Name}) from the daycare.** "
					};
					builder.Append(choices[Random.Next(choices.Length)]);
				}
			}

			if (status.Party != null)
			{
				foreach (Pokemon mon in status.Party)
				{
					if (mon == null) continue;
					uint pv = mon.PersonalityValue;

					List<uint> values =
						oldStatus?.Party?.Where(x => x != null)?.Select(x => x.PersonalityValue)?
							.ToList() ?? new List<uint>();
					List<uint> boxValues = new List<uint>();
					List<Pokemon> pokemon = new List<Pokemon>();
					foreach (List<Pokemon>? p in oldStatus?.PC?.Boxes?.Where(x => x?.BoxContents != null)?.Select(x => x.BoxContents) ?? new List<List<Pokemon>>())
					{
						if (p != null)
							pokemon.AddRange(p);
					}

					values.AddRange(pokemon.Select(x => x.PersonalityValue));
					boxValues.AddRange(pokemon.Select(x => x.PersonalityValue));
					if (oldStatus?.Daycare != null)
					{
						values.AddRange(oldStatus.Daycare.Where(x => x != null).Select(x => x.PersonalityValue));
					}

					if (ReleasedDictionary.Any(x => x.Key.PersonalityValue == mon.PersonalityValue))
					{
						string[] choices =
						{
							$"**We retrieve {mon.Name} ({mon.Species!.Name}) from the PC!** ",
							$"**We withdraw {mon.Name} ({mon.Species.Name}) from the PC!** "
						};
						builder.Append(choices[Random.Next(choices.Length)]);
						Pokemon temp = ReleasedDictionary.First(x => x.Key.PersonalityValue == mon.PersonalityValue)
							.Key;
						ReleasedDictionary.Remove(temp);
					}
					else if (!values.Contains(pv))
					{
						builder.Append(
							$"**Caught a {(mon.Gender != null ? Enum.GetName(typeof(Gender), mon.Gender) + " " : "")}Lv. {mon.Level} {mon.Species!.Name}!** {(mon.Name == mon.Species.Name ? "No nickname. " : $"Nickname: `{mon.Name}`. ")}");
						if (status.Party.All(x => x.PersonalityValue != pv))
						{
							builder.Append("Sent to the PC. ");
						}
					}
					else if (boxValues.Contains(pv))
					{
						string[] choices =
						{
							$"**We retrieve {mon.Name} ({mon.Species!.Name}) from the PC!** ",
							$"**We withdraw {mon.Name} ({mon.Species.Name}) from the PC!** "
						};
						builder.Append(choices[Random.Next(choices.Length)]);
					}
				}
			}

			if (status?.PC?.Boxes != null && oldStatus?.Party != null && oldStatus.Party.Count != 0)
			{
				foreach (Box box in status.PC.Boxes)
				{
					foreach (Pokemon? mon in box.BoxContents!)
					{
						if (mon == null) continue;
						uint pv = mon.PersonalityValue;
						List<uint> values =
							oldStatus?.Party?.Where(x => x != null)?.Select(x => x.PersonalityValue)?.ToList() ?? new List<uint>();
						if (oldStatus?.BattleParty != null)
						{
							foreach (uint id in oldStatus.BattleParty.Where(x => x != null).Select(x => x.PersonalityValue))
							{
								if (!values.Contains(id))
									values.Add(id);
							}
						}
						List<uint> boxValues = new List<uint>();
						List<Pokemon> pokemon = new List<Pokemon>();
						foreach (List<Pokemon>? p in oldStatus?.PC?.Boxes?.Where(x => x?.BoxContents != null)?.Select(x => x.BoxContents) ?? new List<List<Pokemon>>())
						{
							if (p != null)
								pokemon.AddRange(p);
						}

						values.AddRange(pokemon.Select(x => x.PersonalityValue));
						boxValues.AddRange(pokemon.Select(x => x.PersonalityValue));
						if (oldStatus?.Daycare != null)
						{
							values.AddRange(oldStatus.Daycare.Where(x => x != null).Select(x => x.PersonalityValue));
						}

						if (ReleasedDictionary.Any(x => x.Key.PersonalityValue == mon.PersonalityValue))
						{
							string[] choices =
							{
								$"**We deposited {mon.Name} ({mon.Species!.Name}) in the PC!** ",
								$"**We put {mon.Name} ({mon.Species.Name}) in the PC!** ",
								$"**Deposited {mon.Name} ({mon.Species.Name}) in the PC!** "
							};
							builder.Append(choices[Random.Next(choices.Length)]);
							Pokemon temp = ReleasedDictionary.First(x => x.Key.PersonalityValue == mon.PersonalityValue)
								.Key;
							ReleasedDictionary.Remove(temp);
						}
						else if (!values.Contains(pv))
						{
							builder.Append(
								$"**Caught a {(mon.Gender != null ? Enum.GetName(typeof(Gender), mon.Gender) + " " : "")}Lv. {mon.Level} {mon.Species!.Name}!** {(mon.Name == mon.Species.Name ? "No nickname. " : $"Nickname: `{mon.Name}`. ")}");
							builder.Append($"Sent to Box {box.BoxNumber}. ");
						}
					}
				}
			}

			if (status?.BattleParty != null && oldStatus?.BattleParty != null)
			{
				foreach (Pokemon mon in status.BattleParty)
				{
					if (mon == null) continue;
					uint pv = mon.PersonalityValue;
					List<uint> values =
						oldStatus.BattleParty.Where(x => x != null).Select(x => x.PersonalityValue)
							.ToList();

					if (!values.Contains(pv))
					{
						builder.Append(
							$"**Caught a {(mon.Gender != null ? Enum.GetName(typeof(Gender), mon.Gender) + " " : "")}Lv. {mon.Level} {mon.Species!.Name}!** {(mon.Name == mon.Species.Name ? "No nickname. " : $"Nickname: `{mon.Name}` ")}");
						if (oldStatus.BattleParty.Count(x => x != null) == 6)
						{
							builder.Append("Sent to the PC. ");
						}
					}
				}
			}

			if (oldStatus?.PC != null)
			{
				List<Pokemon> oldBoxedMons = new List<Pokemon>();
				foreach (List<Pokemon>? p in oldStatus?.PC?.Boxes?.Where(x => x?.BoxContents != null)?.Select(x => x.BoxContents) ?? new List<List<Pokemon>>())
				{
					if (p != null)
						oldBoxedMons.AddRange(p);
				}

				List<Pokemon> newBoxedMons = new List<Pokemon>();
				foreach (List<Pokemon>? p in status?.PC?.Boxes?.Where(x => x?.BoxContents != null)?.Select(x => x.BoxContents) ?? new List<List<Pokemon>>())
				{
					if (p != null)
						newBoxedMons.AddRange(p);
				}

				foreach (Pokemon mon in newBoxedMons)
				{
					if (oldStatus?.Party?.Any(x => x.PersonalityValue == mon.PersonalityValue) == true ||
						ReleasedDictionary.Any(x => x.Key.PersonalityValue == mon.PersonalityValue) &&
						oldBoxedMons.All(x => x.PersonalityValue != mon.PersonalityValue))
					{
						string[] choices =
						{
							$"**We deposited {mon.Name} ({mon.Species!.Name}) in the PC!** ",
							$"**We put {mon.Name} ({mon.Species.Name}) in the PC!** ",
							$"**Deposited {mon.Name} ({mon.Species.Name}) in the PC!** "
						};
						builder.Append(choices[Random.Next(choices.Length)]);
					}
					if (ReleasedDictionary.Any(x => x.Key.PersonalityValue == mon.PersonalityValue))
					{
						Pokemon temp =
							ReleasedDictionary.First(x => x.Key.PersonalityValue == mon.PersonalityValue).Key;
						ReleasedDictionary.Remove(temp);
					}
				}

				foreach (Pokemon oldMon in oldBoxedMons)
				{
					if ((status?.Party?.All(x => x.PersonalityValue != oldMon.PersonalityValue) ?? true) &&
						newBoxedMons.All(x => x.PersonalityValue != oldMon.PersonalityValue) &&
						(status?.Daycare?.All(x => x.PersonalityValue != oldMon.PersonalityValue) ?? true))
					{
						if (ReleasedDictionary.All(x => x.Key.PersonalityValue != oldMon.PersonalityValue))
							ReleasedDictionary.Add(oldMon, 1);
					}
				}
			}

			if (oldStatus?.Party != null)
			{
				foreach (Pokemon mon in oldStatus.Party)
				{
					List<Pokemon> newBoxedMons = new List<Pokemon>();
					foreach (List<Pokemon>? p in status?.PC?.Boxes?.Where(x => x?.BoxContents != null)?.Select(x => x.BoxContents) ?? new List<List<Pokemon>>())
					{
						if (p != null)
							newBoxedMons.AddRange(p);
					}

					if ((status?.Party?.All(x => x.PersonalityValue != mon.PersonalityValue) ?? true) &&
						newBoxedMons.All(x => x.PersonalityValue != mon.PersonalityValue) &&
						(status?.Daycare?.All(x => x.PersonalityValue != mon.PersonalityValue) ?? true))
					{
						if (ReleasedDictionary.All(x => x.Key.PersonalityValue != mon.PersonalityValue))
							ReleasedDictionary.Add(mon, 1);
					}
				}
			}
			Dictionary<Pokemon, int> releasedCopy = new Dictionary<Pokemon, int>(ReleasedDictionary);
			foreach ((Pokemon mon, int time) in releasedCopy)
			{
				List<uint> values = status?.Party
					?.Select(x => x.PersonalityValue)
					?.ToList() ?? new List<uint>();

				List<Pokemon> mons = new List<Pokemon>();
				foreach (List<Pokemon>? boxedMons in status?.PC?.Boxes?.Where(x => x?.BoxContents != null)?.Select(x => x.BoxContents) ?? new List<List<Pokemon>>())
				{
					if (boxedMons != null)
						mons.AddRange(boxedMons);
				}
				values.AddRange(mons.Select(x => x.PersonalityValue));
				if (oldStatus?.Daycare != null)
				{
					values.AddRange(oldStatus.Daycare.Where(x => x != null).Select(x => x.PersonalityValue));
				}

				if (!values.Contains(mon.PersonalityValue))
				{
					if (time == 6)
					{
						string[] choices =
						{
							$"**WE RELEASE {mon.Name} ({mon.Species!.Name})!** ",
							$"**{mon.Name} ({mon.Species.Name}) HAS BEEN RELEASED! BYE {mon.Name!.ToUpperInvariant()}!** "
						};
						builder.Append(choices[Random.Next(choices.Length)]);
						ReleasedDictionary.Remove(mon);
					}
					else
					{
						ReleasedDictionary[mon] = time + 1;
					}
				}
			}


			if (status?.MapName != oldStatus?.MapName)
			{
				if (status?.MapName == "Pokémon League") //TODO check map name
				{
					List<string> options = new List<string>
					{
						$"**We're locked into the E4 for {(Memory.Urned ? "Rematch " : "")}Attempt #{(Memory.Urned ? Memory.E4RematchNum : Memory.E4AttemptNum)}!** ",
						$"**We're in for E4 {(Memory.Urned ? "Rematch " : "")}Attempt #{(Memory.Urned ? Memory.E4RematchNum : Memory.E4AttemptNum)}!** ",
						$"**Welcome back to the E4! {(Memory.Urned ? "Rematch " : "")}Attempt #{(Memory.Urned ? Memory.E4RematchNum : Memory.E4AttemptNum)}!** ",
						$"**The door slams shut behind us! E4 {(Memory.Urned ? "Rematch " : "")}Attempt #{(Memory.Urned ? Memory.E4RematchNum : Memory.E4AttemptNum)}!** ",
						$"**We stroll boldly into the E4 chambers and are locked inside! {(Memory.Urned ? "Rematch " : "")}Attempt #{(Memory.Urned ? Memory.E4RematchNum : Memory.E4AttemptNum)}!** "
					};
					string message = options[Random.Next(options.Count)];
					builder.Append(message);
					if (Memory.Urned) Memory.E4RematchNum++;
					else Memory.E4AttemptNum++;
				}
				else if (!string.IsNullOrWhiteSpace(status?.MapName))
				{
					string[] move = { "head", "go", "step", "move", "travel", "walk", "stroll", "stride" };
					string choice = move[Random.Next(move.Length)];
					List<string> options = new List<string>
					{
						$"{status.MapName}. ", $"In {status.MapName}. ",
						$"Now in {status.MapName}. ",
						$"We {choice} into {status.MapName}. ",
						$"Arrived at {status.MapName}. "
					};
					string message = options[Random.Next(options.Count)];
					builder.Append(message);
				}
			}

			announcement = aBuilder.ToString().Length == 0 ? null : aBuilder.ToString();
			return builder.ToString().Length == 0 ? null : builder.ToString();
		}
	}
}