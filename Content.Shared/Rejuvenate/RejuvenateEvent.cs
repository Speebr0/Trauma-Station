// SPDX-FileCopyrightText: 2022 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Aviu00 <93730715+Aviu00@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

namespace Content.Shared.Rejuvenate;

/// <summary>
/// Raised when an entity is supposed to be rejuvenated,
/// meaning it should heal all damage, debuffs or other negative status effects.
/// Systems should handle healing the entity in a subscription to this event.
/// Used for the Rejuvenate admin verb.
/// </summary>
// Trauma - change to a struct and add Uncuff + ResetActions
public record struct RejuvenateEvent(bool Uncuff = true, bool ResetActions = true);
