-- phoneme-mapped CV/VC generator
-- outputs: <cyrillic_syllable><two spaces><phoneme_codes separated by spaces>

local phonemap = {
  -- vowels
  ["ae"] = "ә", ["ii"] = "и", ["iu"] = "ү", ["i"] = "і", ["io"] = "ө",
  ["e"]  = "е",  ["a"]  = "а", ["iy"] = "й", ["ou"] = "ұ", ["y"]  = "ы",
  ["o"]  = "о",  ["u"]  = "у",

  -- consonants / digraphs
  ["b"]  = "б", ["g"]  = "г", ["d"]  = "д", ["gh"] = "ғ", ["k"]  = "к",
  ["q"]  = "қ", ["l"]  = "л", ["n"]  = "н", ["r"]  = "р", ["s"]  = "с",
  ["t"]  = "т", ["p"]  = "п", ["m"]  = "м", ["sh"] = "ш", ["j"]  = "ж",
  ["x"]  = "х", ["h"]  = "һ", ["z"]  = "з", ["ng"] = "ң",
}

local groups = {
  v = {
    ji = { "ae", "e", "iu", "i", "io" },   -- жіңішке (thin)
    ju = { "a", "ou", "y", "o" },          -- жуан (thick)
    ne = { "ii", "u" },                    -- neutral (и, у)
  },
  c = {
    ji = { "g", "k" },                     -- consonants tied to thin vowels
    ju = { "gh", "q" },                    -- consonants tied to thick vowels
    ne = { "b","d","l","n","ng","r","s","t","p","m","sh","j","x","h","z" }, -- neutral
  },
}

-- patterns for CV (C then V) and VC (V then C)
local patternsCV =
{
  -- {C_group, V_group}
  { "ne", "ne" }, { "ne", "ji" }, { "ne", "ju" },
  { "ji", "ne" }, { "ju", "ne" },
  { "ne", "ji" }, { "ne", "ju" },
  { "ji", "ji" }, { "ju", "ju" },
  { "ne", "ji" }, { "ne", "ju" },
  { "ji", "ne" }, { "ju", "ne" },
  { "ji", "ji" }, { "ju", "ju" },
}

local patternsVC =
{
  -- {V_group, C_group}
  { "ne", "ne" }, { "ne", "ji" }, { "ne", "ju" },
  { "ji", "ne" }, { "ju", "ne" },
  { "ne", "ji" }, { "ne", "ju" },
  { "ji", "ji" }, { "ju", "ju" },
  { "ne", "ji" }, { "ne", "ju" },
  { "ji", "ne" }, { "ju", "ne" },
  { "ji", "ji" }, { "ju", "ju" },
}

local function join(tbl, sep)
  sep = sep or ""
  local s = ""
  for i = 1, #tbl do
    if i > 1 then s = s .. sep end
    s = s .. tbl[i]
  end
  return s
end

local function code_to_cyr(code)
  return phonemap[code] or code
end

-- print single-phoneme mapping in deterministic order
local function print_single_map()
  local order = {
    "ae","ii","iu","i","io","e","a","iy","ou","y","o","u",
    "b","d","l","n","r","s","t","p","m","sh","j","x","h","z","ng","g","k","gh","q"
  }
  for _, code in ipairs(order) do
    local cyr = code_to_cyr(code)
    if cyr then
      print(cyr .. "  " .. code)
    end
  end
end

-- Generate CV (C + V)
local function gen_CV()
  for _, pat in ipairs(patternsCV) do
    local cgrp, vgrp = pat[1], pat[2]
    for _, ccode in ipairs(groups.c[cgrp]) do
      for _, vcode in ipairs(groups.v[vgrp]) do
        local left = code_to_cyr(ccode) .. code_to_cyr(vcode)
        local right = ccode .. " " .. vcode
        print(left .. "  " .. right)
      end
    end
  end
end

-- Generate VC (V + C)
local function gen_VC()
  for _, pat in ipairs(patternsVC) do
    local vgrp, cgrp = pat[1], pat[2]
    for _, vcode in ipairs(groups.v[vgrp]) do
      for _, ccode in ipairs(groups.c[cgrp]) do
        local left = code_to_cyr(vcode) .. code_to_cyr(ccode)
        local right = vcode .. " " .. ccode
        print(left .. "  " .. right)
      end
    end
  end
end

-- Run: single map, blank line, CV, blank line, VC
print_single_map()
gen_CV()
gen_VC()